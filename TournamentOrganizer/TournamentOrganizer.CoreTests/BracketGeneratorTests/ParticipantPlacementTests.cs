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
    public class ParticipantPlacementTests
    {
        [TestMethod]
        public void GenerateBracket_Round1_FirstParticipantIsPlayerFour()
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
            MatchCoreDto round1Match = rounds[0].Matches.First();
            Assert.AreEqual(
                fiveParticipants[3].Id,
                round1Match.Participant1Id,
                "Round 1 should have Player 4 in first position"
            );
        }

        [TestMethod]
        public void GenerateBracket_Round1_SecondParticipantIsPlayerFive()
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
            MatchCoreDto round1Match = rounds[0].Matches.First();
            Assert.AreEqual(
                fiveParticipants[4].Id,
                round1Match.Participant2Id,
                "Round 1 should have Player 5 in second position"
            );
        }

        [TestMethod]
        public void GenerateBracket_Round2_FirstMatchHasPlayerOneAndTwo()
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
            List<MatchCoreDto> round2Matches = rounds[1].Matches.ToList();
            Assert.AreEqual(
                fiveParticipants[0].Id,
                round2Matches[0].Participant1Id,
                "First match of Round 2 should have Player 1 in first position"
            );
        }

        [TestMethod]
        public void GenerateBracket_Round2_SecondMatchHasPlayerThree()
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
            List<MatchCoreDto> round2Matches = rounds[1].Matches.ToList();
            Assert.AreEqual(
                fiveParticipants[2].Id,
                round2Matches[1].Participant1Id,
                "Second match of Round 2 should have Player 3 in first position"
            );
        }
    }
}
