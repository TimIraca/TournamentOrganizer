using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Services.Implementations.Tests
{
    [TestClass()]
    public class TournamentServiceTests
    {
        [TestMethod]
        public void GenerateBracket_With5Participants_GeneratesCorrectStructure()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();
            var player1Id = Guid.NewGuid();
            var player2Id = Guid.NewGuid();
            var player3Id = Guid.NewGuid();
            var player4Id = Guid.NewGuid();
            var player5Id = Guid.NewGuid();

            var participants = new List<ParticipantCoreDto>
            {
                new ParticipantCoreDto
                {
                    Id = player1Id,
                    Name = "Player 1",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player2Id,
                    Name = "Player 2",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player3Id,
                    Name = "Player 3",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player4Id,
                    Name = "Player 4",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player5Id,
                    Name = "Player 5",
                    TournamentId = tournamentId,
                },
            };

            // Act
            var rounds = TournamentService.GenerateBracket(participants, tournamentId).ToList();

            // Assert
            Assert.AreEqual(3, rounds.Count);

            // Verify Round 1
            var round1 = rounds[0];
            var round1Matches = round1.Matches.ToList();
            Assert.AreEqual(1, round1Matches.Count);
            Assert.AreEqual(player4Id, round1Matches[0].Participant1Id);
            Assert.AreEqual(player5Id, round1Matches[0].Participant2Id);

            // Verify Round 2
            var round2 = rounds[1];
            var round2Matches = round2.Matches.ToList();
            Assert.AreEqual(2, round2Matches.Count);

            // Match 2: Players 2 vs 3
            Assert.AreEqual(player2Id, round2Matches[0].Participant1Id);
            Assert.AreEqual(player3Id, round2Matches[0].Participant2Id);

            // Match 3: Player 1 vs Winner(Match 1)
            Assert.AreEqual(player1Id, round2Matches[1].Participant1Id);
            Assert.IsNull(round2Matches[1].Participant2Id);

            // Verify Round 3 (Finals)
            var round3 = rounds[2];
            var round3Matches = round3.Matches.ToList();
            Assert.AreEqual(1, round3Matches.Count);
            Assert.IsNull(round3Matches[0].Participant1Id);
            Assert.IsNull(round3Matches[0].Participant2Id);
        }

        [TestMethod]
        public void GenerateBracket_WithInvalidParticipantCount_ThrowsArgumentException()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();
            var emptyParticipants = new List<ParticipantCoreDto>();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(
                () => TournamentService.GenerateBracket(emptyParticipants, tournamentId).ToList()
            );
        }

        [TestMethod]
        public void GenerateBracket_WithNullParticipants_ThrowsArgumentNullException()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => TournamentService.GenerateBracket(null, tournamentId).ToList()
            );
        }

        [TestMethod]
        public void GenerateBracket_With5Participants_GeneratesCorrectStructure_AdvancesWinner()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();
            var player1Id = Guid.NewGuid();
            var player2Id = Guid.NewGuid();
            var player3Id = Guid.NewGuid();
            var player4Id = Guid.NewGuid();
            var player5Id = Guid.NewGuid();

            var participants = new List<ParticipantCoreDto>
            {
                new ParticipantCoreDto
                {
                    Id = player1Id,
                    Name = "Player 1",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player2Id,
                    Name = "Player 2",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player3Id,
                    Name = "Player 3",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player4Id,
                    Name = "Player 4",
                    TournamentId = tournamentId,
                },
                new ParticipantCoreDto
                {
                    Id = player5Id,
                    Name = "Player 5",
                    TournamentId = tournamentId,
                },
            };

            // Act
            var rounds = TournamentService.GenerateBracket(participants, tournamentId).ToList();

            // Assert
            Assert.AreEqual(3, rounds.Count);

            // Verify Round 1
            var round1 = rounds[0];
            var round1Matches = round1.Matches.ToList();
            Assert.AreEqual(1, round1Matches.Count);
            Assert.AreEqual(player4Id, round1Matches[0].Participant1Id);
            Assert.AreEqual(player5Id, round1Matches[0].Participant2Id);

            // Verify Round 2
            var round2 = rounds[1];
            var round2Matches = round2.Matches.ToList();
            Assert.AreEqual(2, round2Matches.Count);

            // Match 2: Players 2 vs 3
            Assert.AreEqual(player2Id, round2Matches[0].Participant1Id);
            Assert.AreEqual(player3Id, round2Matches[0].Participant2Id);

            // Match 3: Player 1 vs Winner(Match 1)
            Assert.AreEqual(player1Id, round2Matches[1].Participant1Id);
            Assert.IsNull(round2Matches[1].Participant2Id);

            // Verify Round 3 (Finals)
            var round3 = rounds[2];
            var round3Matches = round3.Matches.ToList();
            Assert.AreEqual(1, round3Matches.Count);
            Assert.IsNull(round3Matches[0].Participant1Id);
            Assert.IsNull(round3Matches[0].Participant2Id);

            // Test progression
            // Simulate Player 4 winning Match 1
            BracketGenerator.UpdateBracket(rounds, player4Id, round1Matches[0].Id);
            Assert.AreEqual(player4Id, round2Matches[1].Participant2Id);

            // Simulate Player 2 winning Match 2
            BracketGenerator.UpdateBracket(rounds, player2Id, round2Matches[0].Id);
            Assert.AreEqual(player2Id, round3Matches[0].Participant1Id);

            // Simulate Player 4 winning Match 3
            BracketGenerator.UpdateBracket(rounds, player4Id, round2Matches[1].Id);
            Assert.AreEqual(player4Id, round3Matches[0].Participant2Id);
        }
    }
}
