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
    public class EightPlayerTournamentTests
    {
        private Guid _tournamentId;
        private List<ParticipantCoreDto> _participants;
        private List<RoundCoreDto> _rounds;

        [TestInitialize]
        public void Setup()
        {
            _tournamentId = Guid.NewGuid();
            _participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = _tournamentId,
                })
                .ToList();
            _rounds = BracketGenerator.GenerateBracket(_participants, _tournamentId).ToList();
        }

        [TestMethod]
        public void GenerateBracket_HasThreeRounds()
        {
            Assert.AreEqual(3, _rounds.Count, "Should have exactly 3 rounds");
        }

        [TestMethod]
        public void GenerateBracket_Round1HasFourMatches()
        {
            Assert.AreEqual(4, _rounds[0].Matches.Count(), "Round 1 should have 4 matches");
        }

        [TestMethod]
        public void GenerateBracket_Round2HasTwoMatches()
        {
            Assert.AreEqual(2, _rounds[1].Matches.Count(), "Round 2 should have 2 matches");
        }

        [TestMethod]
        public void GenerateBracket_Round3HasOneMatch()
        {
            Assert.AreEqual(1, _rounds[2].Matches.Count(), "Round 3 should have 1 match");
        }

        [TestMethod]
        public void UpdateBracket_WinnerAdvancesToCorrectRound2Match()
        {
            // Arrange
            var round1 = _rounds[0];
            var match1Winner = _participants[0].Id;

            // Act
            BracketGenerator.UpdateBracket(_rounds, match1Winner, round1.Matches.ElementAt(0).Id);

            // Assert
            var firstMatchRound2 = _rounds[1].Matches.ElementAt(0);
            Assert.AreEqual(
                match1Winner,
                firstMatchRound2.Participant1Id,
                "Winner of Match 1 should be in first slot of Round 2 Match 1"
            );
        }

        [TestMethod]
        public void UpdateBracket_BothWinnersAdvanceToFinals()
        {
            // Arrange
            var round2 = _rounds[1];
            var semifinal1Winner = _participants[0].Id;
            var semifinal2Winner = _participants[4].Id;

            // Act
            BracketGenerator.UpdateBracket(
                _rounds,
                semifinal1Winner,
                round2.Matches.ElementAt(0).Id
            );
            BracketGenerator.UpdateBracket(
                _rounds,
                semifinal2Winner,
                round2.Matches.ElementAt(1).Id
            );

            // Assert
            var finalMatch = _rounds[2].Matches.First();
            Assert.AreEqual(
                semifinal1Winner,
                finalMatch.Participant1Id,
                "First semifinal winner should advance to first slot in finals"
            );
            Assert.AreEqual(
                semifinal2Winner,
                finalMatch.Participant2Id,
                "Second semifinal winner should advance to second slot in finals"
            );
        }
    }
}
