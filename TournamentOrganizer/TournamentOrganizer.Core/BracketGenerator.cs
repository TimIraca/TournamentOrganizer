using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core
{
    public static class BracketGenerator
    {
        public static IEnumerable<RoundCoreDto> GenerateBracket(
            IEnumerable<ParticipantCoreDto> participants,
            Guid tournamentId
        )
        {
            var participantsList = participants.ToList();
            if (!participantsList.Any())
                throw new ArgumentException(
                    "Must have at least one participant",
                    nameof(participants)
                );

            // Calculate tournament structure
            int totalParticipants = participantsList.Count;
            int totalSlots = GetNextPowerOfTwo(totalParticipants);
            int numberOfByes = totalSlots - totalParticipants;
            int numberOfRounds = (int)Math.Log2(totalSlots);

            var rounds = new List<RoundCoreDto>();
            int matchNumber = 1;

            // Round 1: Create matches for players who need to play in first round
            var playersNeededInFirstRound = (totalParticipants - numberOfByes) & ~1;
            var firstRoundPlayers = participantsList
                .Skip(numberOfByes)
                .Take(playersNeededInFirstRound)
                .ToList();
            var remainingPlayers = participantsList
                .Skip(numberOfByes + playersNeededInFirstRound)
                .ToList();
            var byePlayers = participantsList.Take(numberOfByes).Concat(remainingPlayers).ToList();

            if (firstRoundPlayers.Any())
            {
                var firstRoundMatches = new List<MatchCoreDto>();
                for (int i = 0; i < firstRoundPlayers.Count; i += 2)
                {
                    var match = new MatchCoreDto
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = matchNumber++,
                        Participant1Id = firstRoundPlayers[i].Id,
                        Participant2Id = firstRoundPlayers[i + 1].Id,
                    };
                    firstRoundMatches.Add(match);
                }

                rounds.Add(
                    new RoundCoreDto
                    {
                        Id = Guid.NewGuid(),
                        RoundNumber = 1,
                        TournamentId = tournamentId,
                        Matches = firstRoundMatches,
                    }
                );
            }

            // Round 2: Distribute bye players evenly
            int round2MatchCount = totalSlots / 4;
            var round2Matches = new List<MatchCoreDto>();

            for (int i = 0; i < round2MatchCount; i++)
            {
                var match = new MatchCoreDto { Id = Guid.NewGuid(), MatchNumber = matchNumber++ };

                // Calculate if this match should receive bye players
                int byePlayersForThisMatch = 0;
                if (byePlayers.Count > 0)
                {
                    // Distribute bye players evenly from right to left
                    int remainingMatches = round2MatchCount - i;
                    int remainingByes = byePlayers.Count;
                    byePlayersForThisMatch = Math.Min(
                        2,
                        (remainingByes + remainingMatches - 1) / remainingMatches
                    );
                }

                if (byePlayersForThisMatch > 0)
                {
                    match.Participant1Id = byePlayers[0].Id;
                    byePlayers.RemoveAt(0);

                    if (byePlayersForThisMatch > 1 && byePlayers.Any())
                    {
                        match.Participant2Id = byePlayers[0].Id;
                        byePlayers.RemoveAt(0);
                    }
                }

                round2Matches.Add(match);
            }

            rounds.Add(
                new RoundCoreDto
                {
                    Id = Guid.NewGuid(),
                    RoundNumber = 2,
                    TournamentId = tournamentId,
                    Matches = round2Matches,
                }
            );

            // Generate subsequent rounds
            int matchesInRound = round2MatchCount / 2;
            for (int round = 3; round <= numberOfRounds; round++)
            {
                var matches = new List<MatchCoreDto>();
                for (int i = 0; i < matchesInRound; i++)
                {
                    matches.Add(
                        new MatchCoreDto
                        {
                            Id = Guid.NewGuid(),
                            MatchNumber = matchNumber++,
                            Participant1Id = null,
                            Participant2Id = null,
                        }
                    );
                }

                rounds.Add(
                    new RoundCoreDto
                    {
                        Id = Guid.NewGuid(),
                        RoundNumber = round,
                        TournamentId = tournamentId,
                        Matches = matches,
                    }
                );

                matchesInRound /= 2;
            }

            return rounds;
        }

        public static void UpdateBracket(
            IEnumerable<RoundCoreDto> rounds,
            Guid winnerId,
            Guid matchId
        )
        {
            var roundsList = rounds.ToList();
            var currentRound = roundsList.First(r => r.Matches.Any(m => m.Id == matchId));
            var completedMatch = currentRound.Matches.First(m => m.Id == matchId);
            completedMatch.WinnerId = winnerId;

            var nextRound = roundsList.FirstOrDefault(r =>
                r.RoundNumber == currentRound.RoundNumber + 1
            );
            if (nextRound == null)
                return;

            // Calculate which match in the next round should receive this winner
            int currentMatchIndex = currentRound.Matches.ToList().IndexOf(completedMatch);
            int nextRoundMatchIndex = currentMatchIndex / 2;

            var targetMatch = nextRound.Matches.ElementAt(nextRoundMatchIndex);

            // Check if the target match already has both participants filled
            // If so, find the next available match that can accept a winner
            if (targetMatch.Participant1Id != null && targetMatch.Participant2Id != null)
            {
                targetMatch = nextRound.Matches.FirstOrDefault(m =>
                    m.Participant1Id == null || m.Participant2Id == null
                );
            }

            if (targetMatch == null)
                return;

            // Determine correct slot based on current match number
            if (currentMatchIndex % 2 == 0)
            {
                if (targetMatch.Participant1Id == null)
                    targetMatch.Participant1Id = winnerId;
                else if (targetMatch.Participant2Id == null)
                    targetMatch.Participant2Id = winnerId;
            }
            else
            {
                if (targetMatch.Participant2Id == null)
                    targetMatch.Participant2Id = winnerId;
                else if (targetMatch.Participant1Id == null)
                    targetMatch.Participant1Id = winnerId;
            }
        }

        private static int GetNextPowerOfTwo(int n)
        {
            if (n <= 1)
                return 1;
            n--;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            return n + 1;
        }
    }
}
