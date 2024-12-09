using AutoMapper;
using Moq;
using TournamentOrganizer.Core;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Implementations;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;
using Match = TournamentOrganizer.DAL.Entities.Match;

namespace TournamentOrganizer.CoreTests.Services.Implementations
{
    [TestClass]
    public class MatchServiceTests
    {
        private Mock<IMatchRepository> _mockMatchRepository;
        private Mock<IRoundRepository> _mockRoundRepository;
        private Mock<IMapper> _mockMapper;
        private MatchService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockMatchRepository = new Mock<IMatchRepository>();
            _mockRoundRepository = new Mock<IRoundRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new MatchService(
                _mockMatchRepository.Object,
                _mockMapper.Object,
                _mockRoundRepository.Object
            );
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenMatchExists_ReturnsMatchDto()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var match = new Match { Id = matchId, Participant1Id = Guid.NewGuid() };
            var expectedDto = new MatchCoreDto { Id = matchId };

            _mockMatchRepository.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync(match);
            _mockMapper.Setup(m => m.Map<MatchCoreDto>(match)).Returns(expectedDto);

            // Act
            var result = await _service.GetByIdAsync(matchId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(matchId, result.Id);
            _mockMatchRepository.Verify(r => r.GetByIdAsync(matchId), Times.Once);
            _mockMapper.Verify(m => m.Map<MatchCoreDto>(match), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetByIdAsync_WhenMatchDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            _mockMatchRepository.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync((Match)null);

            // Act
            await _service.GetByIdAsync(matchId);

            // Assert: Exception expected
        }

        [TestMethod]
        public async Task GetAllByRoundIdAsync_ReturnsMatchDtos()
        {
            // Arrange
            var roundId = Guid.NewGuid();
            var matches = new List<Match>
            {
                new Match { Id = Guid.NewGuid() },
                new Match { Id = Guid.NewGuid() },
            };
            var expectedDtos = matches.Select(m => new MatchCoreDto { Id = m.Id }).ToList();

            _mockMatchRepository.Setup(r => r.GetAllByRoundIdAsync(roundId)).ReturnsAsync(matches);
            _mockMapper.Setup(m => m.Map<IEnumerable<MatchCoreDto>>(matches)).Returns(expectedDtos);

            // Act
            var result = await _service.GetAllByRoundIdAsync(roundId);

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(
                expectedDtos.Select(d => d.Id).ToList(),
                result.Select(d => d.Id).ToList()
            );
            _mockMatchRepository.Verify(r => r.GetAllByRoundIdAsync(roundId), Times.Once);
        }

        [TestMethod]
        public async Task AddAsync_ReturnsAddedMatchDto()
        {
            // Arrange
            var inputDto = new MatchCoreDto
            {
                Id = Guid.NewGuid(),
                Participant1Id = Guid.NewGuid(),
                Participant2Id = Guid.NewGuid(),
            };
            var matchEntity = new Match { Id = inputDto.Id };
            var addedMatch = new Match { Id = inputDto.Id };
            var expectedDto = new MatchCoreDto { Id = inputDto.Id };

            _mockMapper.Setup(m => m.Map<Match>(inputDto)).Returns(matchEntity);
            _mockMatchRepository.Setup(r => r.AddAsync(matchEntity)).ReturnsAsync(addedMatch);
            _mockMapper.Setup(m => m.Map<MatchCoreDto>(addedMatch)).Returns(expectedDto);

            // Act
            var result = await _service.AddAsync(inputDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(inputDto.Id, result.Id);
            _mockMatchRepository.Verify(r => r.AddAsync(matchEntity), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenMatchExists_UpdatesMatch()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var inputDto = new MatchCoreDto { Id = matchId };
            var existingMatch = new Match { Id = matchId };
            var matchEntity = new Match { Id = matchId };

            _mockMatchRepository.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync(existingMatch);
            _mockMapper.Setup(m => m.Map<Match>(inputDto)).Returns(matchEntity);

            // Act
            await _service.UpdateAsync(inputDto);

            // Assert
            _mockMatchRepository.Verify(r => r.UpdateAsync(matchEntity), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task UpdateAsync_WhenMatchDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var inputDto = new MatchCoreDto { Id = Guid.NewGuid() };
            _mockMatchRepository.Setup(r => r.GetByIdAsync(inputDto.Id)).ReturnsAsync((Match)null);

            // Act
            await _service.UpdateAsync(inputDto);

            // Assert: Exception expected
        }

        [TestMethod]
        public async Task DeleteAsync_WhenMatchExists_DeletesMatch()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var existingMatch = new Match { Id = matchId };

            _mockMatchRepository.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync(existingMatch);

            // Act
            await _service.DeleteAsync(matchId);

            // Assert
            _mockMatchRepository.Verify(r => r.DeleteAsync(matchId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task DeleteAsync_WhenMatchDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            _mockMatchRepository.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync((Match)null);

            // Act
            await _service.DeleteAsync(matchId);

            // Assert: Exception expected
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task DeclareMatchWinnerAsync_WhenNoRounds_ThrowsNotFoundException()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();
            var matchId = Guid.NewGuid();
            var winnerId = Guid.NewGuid();

            _mockRoundRepository
                .Setup(r => r.GetAllByTournamentIdAsync(tournamentId))
                .ReturnsAsync(new List<Round>());

            // Act
            await _service.DeclareMatchWinnerAsync(tournamentId, matchId, winnerId);

            // Assert: Exception expected
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task DeclareMatchWinnerAsync_WhenWinnerNotInMatch_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();
            var matchId = Guid.NewGuid();
            var winnerId = Guid.NewGuid();
            var match = new Match
            {
                Id = matchId,
                Participant1Id = Guid.NewGuid(),
                Participant2Id = Guid.NewGuid(),
            };
            var rounds = new List<Round> { new Round { Id = Guid.NewGuid() } };

            _mockRoundRepository
                .Setup(r => r.GetAllByTournamentIdAsync(tournamentId))
                .ReturnsAsync(rounds);
            _mockMatchRepository.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync(match);

            // Act
            await _service.DeclareMatchWinnerAsync(tournamentId, matchId, winnerId);

            // Assert: Exception expected
        }
    }
}
