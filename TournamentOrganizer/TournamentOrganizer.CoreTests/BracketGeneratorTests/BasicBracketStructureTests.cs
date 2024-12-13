using TournamentOrganizer.Core;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.CoreTests.BracketGeneratorTests
{
    [TestClass]
    public class BasicBracketStructureTests
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
        public void GenerateBracket_WithFiveParticipants_HasThreeRounds()
        {
            Assert.AreEqual(3, _rounds.Count, "Should have 3 rounds");
        }

        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_Round1HasOneMatch()
        {
            var round1 = _rounds[0];
            Assert.AreEqual(1, round1.Matches.Count(), "Round 1 should have 1 match");
        }

        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_Round2HasTwoMatches()
        {
            var round2 = _rounds[1];
            Assert.AreEqual(2, round2.Matches.Count(), "Round 2 should have 2 matches");
        }

        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_Round3HasOneMatch()
        {
            var round3 = _rounds[2];
            Assert.AreEqual(1, round3.Matches.Count(), "Round 3 should have 1 match");
        }
    }
}
