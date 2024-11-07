using System.ComponentModel.DataAnnotations;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.BLL.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Match> GetMatchAsync(Guid id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                throw new Exception($"Match with ID {id} not found");
            return match;
        }

        public async Task<IEnumerable<Match>> GetTournamentMatchesAsync(Guid tournamentId)
        {
            return await _matchRepository.GetByTournamentIdAsync(tournamentId);
        }

        public async Task<Match> CreateMatchAsync(Match match)
        {
            // Validate match data
            if (match.Participant1Id == match.Participant2Id && match.Participant1Id != null)
                throw new ValidationException("A participant cannot play against themselves");

            match.Status = MatchStatus.Pending;
            match.Id = Guid.NewGuid();

            return await _matchRepository.CreateAsync(match);
        }

        public async Task<Match> UpdateMatchAsync(Guid id, Match match)
        {
            var existingMatch = await GetMatchAsync(id);

            // Update only allowed fields
            existingMatch.ScheduledTime = match.ScheduledTime;
            existingMatch.Participant1Id = match.Participant1Id;
            existingMatch.Participant2Id = match.Participant2Id;

            return await _matchRepository.UpdateAsync(existingMatch);
        }

        public async Task<Match> UpdateScoreAsync(
            Guid id,
            int? participant1Score,
            int? participant2Score
        )
        {
            var match = await GetMatchAsync(id);

            if (match.Status == MatchStatus.Completed)
                throw new InvalidOperationException("Cannot update score of a completed match");

            match.Participant1Score = participant1Score;
            match.Participant2Score = participant2Score;
            match.Status = MatchStatus.InProgress;

            return await _matchRepository.UpdateAsync(match);
        }

        public async Task<Match> CompleteMatchAsync(Guid id, Guid winnerId)
        {
            var match = await GetMatchAsync(id);

            if (match.Status == MatchStatus.Completed)
                throw new InvalidOperationException("Match is already completed");

            if (winnerId != match.Participant1Id && winnerId != match.Participant2Id)
                throw new ValidationException("Winner must be one of the match participants");

            match.Status = MatchStatus.Completed;
            match.WinnerId = winnerId;

            return await _matchRepository.UpdateAsync(match);
        }

        public async Task DeleteMatchAsync(Guid id)
        {
            var match = await GetMatchAsync(id);
            await _matchRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Match>> GenerateSingleEliminationBracketAsync(
            Guid tournamentId,
            IList<TournamentParticipant> participants
        )
        {
            if (!participants.Any())
                throw new ValidationException("Cannot generate bracket with no participants");

            // Calculate the number of rounds needed
            int participantCount = participants.Count;
            int totalRounds = (int)Math.Ceiling(Math.Log(participantCount, 2));
            int totalMatches = (int)Math.Pow(2, totalRounds) - 1;

            // Calculate byes needed (if any)
            int perfectBracketSize = (int)Math.Pow(2, totalRounds);
            int byeCount = perfectBracketSize - participantCount;

            // Shuffle participants to randomize the bracket
            var shuffledParticipants = participants.OrderBy(x => Guid.NewGuid()).ToList();

            // Create all matches for the tournament
            var matches = new List<Match>();
            int currentRound = 1;
            int matchesInFirstRound = perfectBracketSize / 2;

            // Generate first round matches
            for (int i = 0; i < matchesInFirstRound; i++)
            {
                var match = new Match
                {
                    Id = Guid.NewGuid(),
                    TournamentId = tournamentId,
                    Round = currentRound,
                    Status = MatchStatus.Pending,
                    ScheduledTime = null,
                };

                // Assign participants, handling byes
                if (i < participantCount / 2)
                {
                    match.Participant1Id = shuffledParticipants[i * 2].Id;

                    if ((i * 2 + 1) < participantCount)
                        match.Participant2Id = shuffledParticipants[i * 2 + 1].Id;
                    else
                    {
                        // This is a bye match
                        match.Status = MatchStatus.Completed;
                        match.WinnerId = match.Participant1Id;
                    }
                }

                matches.Add(match);
            }

            // Generate placeholder matches for subsequent rounds
            currentRound++;
            int matchesInRound = matchesInFirstRound / 2;

            while (matchesInRound > 0)
            {
                for (int i = 0; i < matchesInRound; i++)
                {
                    var match = new Match
                    {
                        Id = Guid.NewGuid(),
                        TournamentId = tournamentId,
                        Round = currentRound,
                        Status = MatchStatus.Pending,
                    };
                    matches.Add(match);
                }

                matchesInRound /= 2;
                currentRound++;
            }

            // Save all matches to database
            foreach (var match in matches)
            {
                await _matchRepository.CreateAsync(match);
            }

            return matches;
        }

        public async Task<IEnumerable<Match>> GetBracketStructureAsync(Guid tournamentId)
        {
            var matches = await _matchRepository.GetByTournamentIdAsync(tournamentId);
            return matches.OrderBy(m => m.Round).ThenBy(m => m.Id); // Consistent ordering within rounds
        }

        // Add method to advance winners
        public async Task AdvanceWinnerAsync(Guid matchId)
        {
            var match = await GetMatchAsync(matchId);
            if (match.Status != MatchStatus.Completed)
                throw new InvalidOperationException("Cannot advance winner from incomplete match");

            if (!match.WinnerId.HasValue)
                throw new InvalidOperationException("No winner specified for the match");

            // Find the next match in the bracket
            var allMatches = await GetBracketStructureAsync(match.TournamentId);
            var nextRoundMatches = allMatches.Where(m => m.Round == match.Round + 1).ToList();

            if (!nextRoundMatches.Any())
                return; // This was the final match

            // Calculate position in current round to determine next match
            var currentRoundMatches = allMatches.Where(m => m.Round == match.Round).ToList();
            var matchIndex = currentRoundMatches.IndexOf(match);
            var nextMatchIndex = matchIndex / 2;
            var nextMatch = nextRoundMatches[nextMatchIndex];

            // Determine if winner should be Participant1 or Participant2 in next match
            if (matchIndex % 2 == 0)
                nextMatch.Participant1Id = match.WinnerId;
            else
                nextMatch.Participant2Id = match.WinnerId;

            await _matchRepository.UpdateAsync(nextMatch);
        }
    }
}
