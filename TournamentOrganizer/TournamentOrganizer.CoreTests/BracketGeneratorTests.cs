using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Tests
{
    [TestClass]
    public class BracketGeneratorTests
    {
        private Guid _tournamentId;

        [TestInitialize]
        public void Setup()
        {
            _tournamentId = Guid.NewGuid();
        }

        [TestMethod]
        public void GenerateBracket_WithFiveParticipants_CorrectStructure()
        {
            // Arrange
            var participants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                participants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = _tournamentId,
                    }
                );
            }

            // Act
            var rounds = BracketGenerator.GenerateBracket(participants, _tournamentId).ToList();

            // Assert
            Assert.AreEqual(3, rounds.Count, "Should have 3 rounds");

            // Round 1
            var round1 = rounds[0];
            Assert.AreEqual(1, round1.Matches.Count(), "Round 1 should have 1 match");
            var round1Match = round1.Matches.First();
            Assert.AreEqual(
                participants[3].Id,
                round1Match.Participant1Id,
                "Round 1 should have Player 4"
            );
            Assert.AreEqual(
                participants[4].Id,
                round1Match.Participant2Id,
                "Round 1 should have Player 5"
            );

            // Round 2
            var round2 = rounds[1];
            Assert.AreEqual(2, round2.Matches.Count(), "Round 2 should have 2 matches");
            var round2Matches = round2.Matches.ToList();

            // First match should have Players 1 and 2
            Assert.AreEqual(
                participants[0].Id,
                round2Matches[0].Participant1Id,
                "First Round 2 match should have Player 1"
            );
            Assert.AreEqual(
                participants[1].Id,
                round2Matches[0].Participant2Id,
                "First Round 2 match should have Player 2"
            );

            // Second match should have Player 3 waiting for Round 1 winner
            Assert.AreEqual(
                participants[2].Id,
                round2Matches[1].Participant1Id,
                "Second Round 2 match should have Player 3"
            );
            Assert.IsNull(
                round2Matches[1].Participant2Id,
                "Second Round 2 match should await Round 1 winner"
            );

            // Round 3 (Finals)
            var round3 = rounds[2];
            Assert.AreEqual(1, round3.Matches.Count(), "Round 3 should have 1 match");
            var finalMatch = round3.Matches.First();
            Assert.IsNull(finalMatch.Participant1Id, "Finals should await first Round 2 winner");
            Assert.IsNull(finalMatch.Participant2Id, "Finals should await second Round 2 winner");
        }

        [TestMethod]
        public void UpdateBracket_WithFiveParticipants_CorrectProgression()
        {
            // Arrange
            var participants = new List<ParticipantCoreDto>();
            for (int i = 1; i <= 5; i++)
            {
                participants.Add(
                    new ParticipantCoreDto
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Player {i}",
                        TournamentId = _tournamentId,
                    }
                );
            }

            var rounds = BracketGenerator.GenerateBracket(participants, _tournamentId).ToList();

            // Act - Simulate tournament progression

            // Round 1: Player 4 wins
            var round1Match = rounds[0].Matches.First();
            BracketGenerator.UpdateBracket(rounds, participants[3].Id, round1Match.Id);

            // Verify Round 2 second match updated correctly
            var round2Matches = rounds[1].Matches.ToList();
            Assert.AreEqual(
                participants[3].Id,
                round2Matches[1].Participant2Id,
                "Winner from Round 1 should advance to second match of Round 2"
            );

            // Round 2: Players 1 and 4 win their matches
            BracketGenerator.UpdateBracket(rounds, participants[0].Id, round2Matches[0].Id);
            BracketGenerator.UpdateBracket(rounds, participants[3].Id, round2Matches[1].Id);

            // Verify Finals populated correctly
            var finalMatch = rounds[2].Matches.First();
            Assert.AreEqual(
                participants[0].Id,
                finalMatch.Participant1Id,
                "First Round 2 winner should advance to first slot in finals"
            );
            Assert.AreEqual(
                participants[3].Id,
                finalMatch.Participant2Id,
                "Second Round 2 winner should advance to second slot in finals"
            );
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        public void GenerateBracket_VariousParticipantCounts_ValidBracket(int participantCount)
        {
            // Arrange
            var participants = Enumerable
                .Range(1, participantCount)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = _tournamentId,
                })
                .ToList();

            // Act
            var rounds = BracketGenerator.GenerateBracket(participants, _tournamentId).ToList();

            // Assert
            Assert.IsTrue(rounds.Count > 0, "Should generate at least one round");
            Assert.IsTrue(rounds.All(r => r.Matches.Any()), "All rounds should have matches");

            // Verify all participants are included
            var allParticipantIds = rounds
                .SelectMany(r => r.Matches)
                .SelectMany(m => new[] { m.Participant1Id, m.Participant2Id })
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .Distinct();

            Assert.AreEqual(
                participantCount,
                allParticipantIds.Count(),
                "All participants should be assigned to matches"
            );
        }

        [TestMethod]
        public void EightPlayerTournament_CorrectProgressionTest()
        {
            // Arrange
            var participants = new List<ParticipantCoreDto>
            {
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 1" },
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 2" },
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 3" },
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 4" },
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 5" },
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 6" },
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 7" },
                new ParticipantCoreDto { Id = Guid.NewGuid(), Name = "Player 8" },
            };

            var tournamentId = Guid.NewGuid();

            // Act
            var rounds = BracketGenerator.GenerateBracket(participants, tournamentId).ToList();

            // Assert initial bracket structure
            Assert.AreEqual(3, rounds.Count, "Should have exactly 3 rounds");
            Assert.AreEqual(4, rounds[0].Matches.Count(), "Round 1 should have 4 matches");
            Assert.AreEqual(2, rounds[1].Matches.Count(), "Round 2 should have 2 matches");
            Assert.AreEqual(1, rounds[2].Matches.Count(), "Round 3 should have 1 match");

            // Verify initial player assignments in Round 1
            var round1 = rounds[0];
            Assert.IsNotNull(
                round1.Matches.ElementAt(0).Participant1Id,
                "Match 1 should have Player 1"
            );
            Assert.IsNotNull(
                round1.Matches.ElementAt(0).Participant2Id,
                "Match 1 should have Player 2"
            );
            Assert.IsNotNull(
                round1.Matches.ElementAt(1).Participant1Id,
                "Match 2 should have Player 3"
            );
            Assert.IsNotNull(
                round1.Matches.ElementAt(1).Participant2Id,
                "Match 2 should have Player 4"
            );
            Assert.IsNotNull(
                round1.Matches.ElementAt(2).Participant1Id,
                "Match 3 should have Player 5"
            );
            Assert.IsNotNull(
                round1.Matches.ElementAt(2).Participant2Id,
                "Match 3 should have Player 6"
            );
            Assert.IsNotNull(
                round1.Matches.ElementAt(3).Participant1Id,
                "Match 4 should have Player 7"
            );
            Assert.IsNotNull(
                round1.Matches.ElementAt(3).Participant2Id,
                "Match 4 should have Player 8"
            );

            // Test progression - Winners from Matches 1 & 2 should go to first Match in Round 2
            var match1Winner = participants[0].Id;
            var match2Winner = participants[2].Id;
            var match3Winner = participants[4].Id;
            var match4Winner = participants[6].Id;

            BracketGenerator.UpdateBracket(rounds, match3Winner, round1.Matches.ElementAt(2).Id);
            // Simulate Match 1 winner
            BracketGenerator.UpdateBracket(rounds, match1Winner, round1.Matches.ElementAt(0).Id);
            // Simulate Match 2 winner
            BracketGenerator.UpdateBracket(rounds, match2Winner, round1.Matches.ElementAt(1).Id);
            // Simulate Match 3 winner

            // Simulate Match 4 winner
            BracketGenerator.UpdateBracket(rounds, match4Winner, round1.Matches.ElementAt(3).Id);

            // Verify Round 2 progression
            var round2 = rounds[1];
            var firstMatchRound2 = round2.Matches.ElementAt(0);
            var secondMatchRound2 = round2.Matches.ElementAt(1);

            // Winners from Matches 1 & 2 should be in first Match of Round 2
            Assert.AreEqual(
                match1Winner,
                firstMatchRound2.Participant1Id,
                "Winner of Match 1 should be in first slot of Round 2 Match 1"
            );
            Assert.AreEqual(
                match2Winner,
                firstMatchRound2.Participant2Id,
                "Winner of Match 2 should be in second slot of Round 2 Match 1"
            );

            // Winners from Matches 3 & 4 should be in second Match of Round 2
            Assert.AreEqual(
                match3Winner,
                secondMatchRound2.Participant1Id,
                "Winner of Match 3 should be in first slot of Round 2 Match 2"
            );
            Assert.AreEqual(
                match4Winner,
                secondMatchRound2.Participant2Id,
                "Winner of Match 4 should be in second slot of Round 2 Match 2"
            );
        }
    }
}
