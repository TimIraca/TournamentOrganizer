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
        public void GenerateBracket_Round1_FirstParticipantIsPlayerFour()
        {
            var round1Match = _rounds[0].Matches.First();
            Assert.AreEqual(
                _fiveParticipants[3].Id,
                round1Match.Participant1Id,
                "Round 1 should have Player 4 in first position"
            );
        }

        [TestMethod]
        public void GenerateBracket_Round1_SecondParticipantIsPlayerFive()
        {
            var round1Match = _rounds[0].Matches.First();
            Assert.AreEqual(
                _fiveParticipants[4].Id,
                round1Match.Participant2Id,
                "Round 1 should have Player 5 in second position"
            );
        }

        [TestMethod]
        public void GenerateBracket_Round2_FirstMatchHasPlayerOneAndTwo()
        {
            var round2Matches = _rounds[1].Matches.ToList();
            Assert.AreEqual(
                _fiveParticipants[0].Id,
                round2Matches[0].Participant1Id,
                "First match of Round 2 should have Player 1 in first position"
            );
        }

        [TestMethod]
        public void GenerateBracket_Round2_SecondMatchHasPlayerThree()
        {
            var round2Matches = _rounds[1].Matches.ToList();
            Assert.AreEqual(
                _fiveParticipants[2].Id,
                round2Matches[1].Participant1Id,
                "Second match of Round 2 should have Player 3 in first position"
            );
        }
    }
}
