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
        [TestInitialize]
        public void Setup()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();
        }

        [TestMethod]
        public void GenerateBracket_HasThreeRounds()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();

            Assert.AreEqual(3, rounds.Count, "Should have exactly 3 rounds");
        }

        [TestMethod]
        public void GenerateBracket_Round1HasFourMatches()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();

            Assert.AreEqual(4, rounds[0].Matches.Count(), "Round 1 should have 4 matches");
        }

        [TestMethod]
        public void GenerateBracket_Round2HasTwoMatches()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();

            Assert.AreEqual(2, rounds[1].Matches.Count(), "Round 2 should have 2 matches");
        }

        [TestMethod]
        public void GenerateBracket_Round3HasOneMatch()
        {
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();

            Assert.AreEqual(1, rounds[2].Matches.Count(), "Round 3 should have 1 match");
        }

        [TestMethod]
        public void UpdateBracket_WinnerAdvancesToCorrectRound2Match()
        {
            // Arrange
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();
            RoundCoreDto round1 = rounds[0];
            Guid match1Winner = participants[0].Id;

            // Act
            BracketGenerator.UpdateBracket(rounds, match1Winner, round1.Matches.ElementAt(0).Id);

            // Assert
            MatchCoreDto firstMatchRound2 = rounds[1].Matches.ElementAt(0);
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
            Guid tournamentId = Guid.NewGuid();
            List<ParticipantCoreDto> participants = Enumerable
                .Range(1, 8)
                .Select(i => new ParticipantCoreDto
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i}",
                    TournamentId = tournamentId,
                })
                .ToList();
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participants, tournamentId)
                .ToList();
            RoundCoreDto round2 = rounds[1];
            Guid semifinal1Winner = participants[0].Id;
            Guid semifinal2Winner = participants[4].Id;

            // Act
            BracketGenerator.UpdateBracket(
                rounds,
                semifinal1Winner,
                round2.Matches.ElementAt(0).Id
            );
            BracketGenerator.UpdateBracket(
                rounds,
                semifinal2Winner,
                round2.Matches.ElementAt(1).Id
            );

            // Assert
            MatchCoreDto finalMatch = rounds[2].Matches.First();
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
