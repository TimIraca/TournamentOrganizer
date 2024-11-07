using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TournamentOrganizer.Domain.DTOs;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.mapping;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.BLL.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _repository;
        private readonly IMapper _mapper;

        public TournamentService(ITournamentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TournamentDto>> GetAllAsync()
        {
            var tournaments = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
        }

        public async Task<TournamentDto> GetByIdAsync(Guid id)
        {
            var tournament = await _repository.GetByIdAsync(id);
            if (tournament == null)
                return null;

            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<TournamentDto> CreateAsync(CreateTournamentDto createDto)
        {
            var tournament = _mapper.Map<Tournament>(createDto);
            tournament.Id = Guid.NewGuid();
            tournament.Status = TournamentStatus.Registration;

            var created = await _repository.CreateAsync(tournament);
            return _mapper.Map<TournamentDto>(created);
        }

        public async Task<TournamentDto> UpdateAsync(Guid id, UpdateTournamentDto updateDto)
        {
            var tournament = await _repository.GetByIdAsync(id);
            if (tournament == null)
                return null;

            if (tournament.Status == TournamentStatus.Registration)
            {
                tournament.Description = updateDto.Description;
                tournament.StartDate = updateDto.StartDate;
                tournament.MaxParticipants = updateDto.MaxParticipants;
                tournament.PrizePool = updateDto.PrizePool;
                tournament.PrizeCurrency = updateDto.PrizeCurrency;
            }

            await _repository.UpdateAsync(tournament);
            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tournament = await _repository.GetByIdAsync(id);
            if (tournament == null)
                return false;

            if (tournament.Status != TournamentStatus.Registration)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<ParticipantDto> RegisterParticipantAsync(
            Guid tournamentId,
            RegisterParticipantDto registerDto
        )
        {
            var tournament = await _repository.GetByIdWithParticipantsAsync(tournamentId);

            if (
                tournament == null
                || tournament.Status != TournamentStatus.Registration
                || tournament.Participants.Count >= tournament.MaxParticipants
            )
            {
                return null;
            }

            var participant = new TournamentParticipant
            {
                Id = Guid.NewGuid(),
                TournamentId = tournamentId,
                ParticipantName = registerDto.ParticipantName,
                RegistrationDate = DateTime.UtcNow,
            };

            if (tournament.Participants == null)
            {
                tournament.Participants = new List<TournamentParticipant>();
            }

            tournament.Participants.Add(participant);

            try
            {
                await _repository.UpdateAsync(tournament);
                return _mapper.Map<ParticipantDto>(participant);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> RemoveParticipantAsync(Guid tournamentId, Guid participantId)
        {
            var tournament = await _repository.GetByIdWithParticipantsAsync(tournamentId);
            if (tournament == null || tournament.Status != TournamentStatus.Registration)
                return false;

            var participant = tournament.Participants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
                return false;

            tournament.Participants.Remove(participant);
            await _repository.UpdateAsync(tournament);
            return true;
        }

        public async Task<bool> StartTournamentAsync(Guid id)
        {
            var tournament = await _repository.GetByIdWithParticipantsAsync(id);
            if (
                tournament == null
                || tournament.Status != TournamentStatus.Registration
                || tournament.Participants.Count < 2
            )
            {
                return false;
            }

            tournament.Status = TournamentStatus.InProgress;
            tournament.StartDate = DateTime.UtcNow;

            var matches = GenerateMatches(tournament);
            tournament.Matches.AddRange(matches);

            await _repository.UpdateAsync(tournament);
            return true;
        }

        public async Task<bool> EndTournamentAsync(Guid id)
        {
            var tournament = await _repository.GetByIdWithDetailsAsync(id);
            if (tournament == null || tournament.Status != TournamentStatus.InProgress)
                return false;

            if (tournament.Matches.Any(m => m.Status != MatchStatus.Completed))
                return false;

            tournament.Status = TournamentStatus.Completed;
            tournament.EndDate = DateTime.UtcNow;

            await _repository.UpdateAsync(tournament);
            return true;
        }

        public async Task<IEnumerable<ParticipantDto>> GetParticipantsAsync(Guid tournamentId)
        {
            var tournament = await _repository.GetByIdWithParticipantsAsync(tournamentId);
            if (tournament == null)
                return null;

            return _mapper.Map<IEnumerable<ParticipantDto>>(tournament.Participants);
        }

        public async Task<IEnumerable<MatchDto>> GetMatchesAsync(Guid tournamentId)
        {
            var tournament = await _repository.GetByIdWithDetailsAsync(tournamentId);
            if (tournament == null)
                return null;

            return _mapper.Map<IEnumerable<MatchDto>>(tournament.Matches);
        }

        public async Task<bool> UpdateMatchScoreAsync(
            Guid tournamentId,
            Guid matchId,
            UpdateMatchScoreDto scoreDto
        )
        {
            var tournament = await _repository.GetByIdWithDetailsAsync(tournamentId);
            if (tournament == null || tournament.Status != TournamentStatus.InProgress)
                return false;

            var match = tournament.Matches.FirstOrDefault(m => m.Id == matchId);
            if (match == null || match.Status == MatchStatus.Completed)
                return false;

            match.Participant1Score = scoreDto.Participant1Score;
            match.Participant2Score = scoreDto.Participant2Score;
            match.Status = MatchStatus.Completed;
            match.WinnerId =
                scoreDto.Participant1Score > scoreDto.Participant2Score
                    ? match.Participant1Id
                    : match.Participant2Id;

            await _repository.UpdateAsync(tournament);
            return true;
        }

        private List<Match> GenerateMatches(Tournament tournament)
        {
            var matches = new List<Match>();
            var participants = tournament.Participants.ToList();

            switch (tournament.Format)
            {
                case TournamentFormat.SingleElimination:
                    matches.AddRange(GenerateSingleEliminationMatches(participants));
                    break;
                case TournamentFormat.RoundRobin:
                    matches.AddRange(GenerateRoundRobinMatches(participants));
                    break;
            }

            return matches;
        }

        private List<Match> GenerateSingleEliminationMatches(
            List<TournamentParticipant> participants
        )
        {
            var matches = new List<Match>();
            var round = 1;
            var matchups = participants.Count / 2;

            for (int i = 0; i < matchups; i++)
            {
                matches.Add(
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        Round = round,
                        Participant1Id = participants[i * 2].Id,
                        Participant2Id = participants[i * 2 + 1].Id,
                        Status = MatchStatus.Pending,
                        ScheduledTime = DateTime.UtcNow.AddHours(i),
                    }
                );
            }

            return matches;
        }

        private List<Match> GenerateRoundRobinMatches(List<TournamentParticipant> participants)
        {
            var matches = new List<Match>();
            var round = 1;

            for (int i = 0; i < participants.Count; i++)
            {
                for (int j = i + 1; j < participants.Count; j++)
                {
                    matches.Add(
                        new Match
                        {
                            Id = Guid.NewGuid(),
                            Round = round,
                            Participant1Id = participants[i].Id,
                            Participant2Id = participants[j].Id,
                            Status = MatchStatus.Pending,
                            ScheduledTime = DateTime.UtcNow.AddHours(matches.Count),
                        }
                    );
                }
                round++;
            }

            return matches;
        }
    }
}
