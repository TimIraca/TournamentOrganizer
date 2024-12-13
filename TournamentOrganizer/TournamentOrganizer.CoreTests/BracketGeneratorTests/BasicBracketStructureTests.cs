using TournamentOrganizer.Core;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.CoreTests.BracketGeneratorTests
{
    [TestClass]
    public class BasicBracketStructureTests
    {
        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_HasThreeRounds()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = tournamentId,
                    }
                );
            }
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(fiveParticipants, tournamentId)
                .ToList();
            Assert.AreEqual(3, rounds.Count, "Should have 3 rounds");
        }

        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_Round1HasOneMatch()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = tournamentId,
                    }
                );
            }
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(fiveParticipants, tournamentId)
                .ToList();
            RoundCoreDto round1 = rounds[0];
            Assert.AreEqual(1, round1.Matches.Count(), "Round 1 should have 1 match");
        }

        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_Round2HasTwoMatches()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = tournamentId,
                    }
                );
            }
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(fiveParticipants, tournamentId)
                .ToList();
            RoundCoreDto round2 = rounds[1];
            Assert.AreEqual(2, round2.Matches.Count(), "Round 2 should have 2 matches");
        }

        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_Round3HasOneMatch()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> fiveParticipants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                fiveParticipants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = tournamentId,
                    }
                );
            }
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(fiveParticipants, tournamentId)
                .ToList();
            RoundCoreDto round3 = rounds[2];
            Assert.AreEqual(1, round3.Matches.Count(), "Round 3 should have 1 match");
        }
    }
}
