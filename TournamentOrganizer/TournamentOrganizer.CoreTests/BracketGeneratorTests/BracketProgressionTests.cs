using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.CoreTests.BracketGeneratorTests
{
    [TestClass]
    public class BracketProgressionTests
    {
        [TestInitialize]
        public void Setup()
        {
            Guid _tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> _fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                _fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = _tournamentId,
                    }
                );
            }
        }

        [TestMethod]
        public void UpdateBracket_WhenPlayerFourWinsRound1_AdvancesToRound2()
        {
            // Arrange
            Guid _tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> _fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                _fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = _tournamentId,
                    }
                );
            }
            List<RoundCoreDto> _rounds = BracketGenerator
                .GenerateBracket(_fiveParticipants, _tournamentId)
                .ToList();
            MatchCoreDto round1Match = _rounds[0].Matches.First();

            // Act
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[3].Id, round1Match.Id);

            // Assert
            List<MatchCoreDto> round2Matches = _rounds[1].Matches.ToList();
            Assert.AreEqual(
                _fiveParticipants[3].Id,
                round2Matches[1].Participant2Id,
                "Player 4 should advance to second match of Round 2"
            );
        }

        [TestMethod]
        public void UpdateBracket_WhenPlayerOneWinsRound2_AdvancesToFinals()
        {
            // Arrange
            Guid _tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> _fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                _fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = _tournamentId,
                    }
                );
            }
            List<RoundCoreDto> _rounds = BracketGenerator
                .GenerateBracket(_fiveParticipants, _tournamentId)
                .ToList();
            List<MatchCoreDto> round2Matches = _rounds[1].Matches.ToList();

            // Act
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[0].Id, round2Matches[0].Id);

            // Assert
            MatchCoreDto finalMatch = _rounds[2].Matches.First();
            Assert.AreEqual(
                _fiveParticipants[0].Id,
                finalMatch.Participant1Id,
                "Player 1 should advance to first slot in finals"
            );
        }

        [TestMethod]
        public void UpdateBracket_WhenPlayerFourWinsSemifinal_AdvancesToFinals()
        {
            // Arrange
            Guid _tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> _fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                _fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = _tournamentId,
                    }
                );
            }
            List<RoundCoreDto> _rounds = BracketGenerator
                .GenerateBracket(_fiveParticipants, _tournamentId)
                .ToList();
            MatchCoreDto round1Match = _rounds[0].Matches.First();
            List<MatchCoreDto> round2Matches = _rounds[1].Matches.ToList();
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[3].Id, round1Match.Id);

            // Act
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[3].Id, round2Matches[1].Id);

            // Assert
            MatchCoreDto finalMatch = _rounds[2].Matches.First();
            Assert.AreEqual(
                _fiveParticipants[3].Id,
                finalMatch.Participant2Id,
                "Player 4 should advance to second slot in finals"
            );
        }
    }
}
