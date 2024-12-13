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
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = CreateParticipants(
                participantCount,
                tournamentId
            );

            // Act
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();

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
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = CreateParticipants(
                participantCount,
                tournamentId
            );

            // Act
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();

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
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = CreateParticipants(
                participantCount,
                tournamentId
            );

            // Act
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();

            // Assert
            IEnumerable<Guid> allParticipantIds = rounds
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

        private static List<ParticipantCoreDto> CreateParticipants(int count, Guid tournamentId)
        {
            return Enumerable
                .Range(1, count)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
        }
    }
}
