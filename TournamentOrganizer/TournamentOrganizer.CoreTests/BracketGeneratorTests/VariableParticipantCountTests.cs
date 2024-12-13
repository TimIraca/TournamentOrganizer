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
    public class VariableParticipantCountTests
    {
        private Guid _tournamentId;

        [TestInitialize]
        public void Setup()
        {
            _tournamentId = Guid.NewGuid();
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        public void GenerateBracket_HasAtLeastOneRound(int participantCount)
        {
            // Arrange
            var participants = CreateParticipants(participantCount);

            // Act
            var rounds = BracketGenerator.GenerateBracket(participants, _tournamentId).ToList();

            // Assert
            Assert.IsTrue(rounds.Count > 0, "Should generate at least one round");
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        public void GenerateBracket_AllRoundsHaveMatches(int participantCount)
        {
            // Arrange
            var participants = CreateParticipants(participantCount);

            // Act
            var rounds = BracketGenerator.GenerateBracket(participants, _tournamentId).ToList();

            // Assert
            Assert.IsTrue(
                rounds.All(r => r.Matches.Any()),
                "All rounds should have at least one match"
            );
        }

        [TestMethod]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        public void GenerateBracket_IncludesAllParticipants(int participantCount)
        {
            // Arrange
            var participants = CreateParticipants(participantCount);

            // Act
            var rounds = BracketGenerator.GenerateBracket(participants, _tournamentId).ToList();

            // Assert
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

        private List<ParticipantCoreDto> CreateParticipants(int count)
        {
            return Enumerable
                .Range(1, count)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = _tournamentId,
                })
                .ToList();
        }
    }
}
