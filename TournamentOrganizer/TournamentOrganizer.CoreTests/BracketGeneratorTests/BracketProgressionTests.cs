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
        private Guid _tournamentId;
        private List<ParticipantCoreDto> _fiveParticipants;
        private List<RoundCoreDto> _rounds;

        [TestInitialize]
        public void Setup()
        {
            _tournamentId = Guid.NewGuid();
            _fiveParticipants = new List<ParticipantCoreDto>();
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
            _rounds = BracketGenerator.GenerateBracket(_fiveParticipants, _tournamentId).ToList();
        }

        [TestMethod]
        public void UpdateBracket_WhenPlayerFourWinsRound1_AdvancesToRound2()
        {
            // Arrange
            var round1Match = _rounds[0].Matches.First();

            // Act
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[3].Id, round1Match.Id);

            // Assert
            var round2Matches = _rounds[1].Matches.ToList();
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
            var round2Matches = _rounds[1].Matches.ToList();

            // Act
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[0].Id, round2Matches[0].Id);

            // Assert
            var finalMatch = _rounds[2].Matches.First();
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
            var round1Match = _rounds[0].Matches.First();
            var round2Matches = _rounds[1].Matches.ToList();
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[3].Id, round1Match.Id);

            // Act
            BracketGenerator.UpdateBracket(_rounds, _fiveParticipants[3].Id, round2Matches[1].Id);

            // Assert
            var finalMatch = _rounds[2].Matches.First();
            Assert.AreEqual(
                _fiveParticipants[3].Id,
                finalMatch.Participant2Id,
                "Player 4 should advance to second slot in finals"
            );
        }
    }
}
