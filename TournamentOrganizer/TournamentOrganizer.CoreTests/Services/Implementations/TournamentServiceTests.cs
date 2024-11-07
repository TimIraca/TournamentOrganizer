using AutoMapper;
using Moq;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;

namespace TournamentOrganizer.Core.Services.Implementations.Tests
{
    [TestClass]
    public class TournamentServiceTests
    {
        private Mock<IParticipantRepository> _participantRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ParticipantService _participantService;

        [TestInitialize]
        public void Setup()
        {
            _participantRepositoryMock = new Mock<IParticipantRepository>();
            _mapperMock = new Mock<IMapper>();
            _participantService = new ParticipantService(
                _participantRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddParticipant_WhenBelowMaxLimit()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();
            var participantDto = new TournamentParticipantCoreDto { TournamentId = tournamentId };
            var participant = new TournamentParticipant { TournamentId = tournamentId };

            _participantRepositoryMock
                .Setup(repo => repo.CountParticipantsAsync(tournamentId))
                .ReturnsAsync(3);
            _participantRepositoryMock
                .Setup(repo => repo.GetTournamentAsync(tournamentId))
                .ReturnsAsync(new Tournament { Id = tournamentId, MaxParticipants = 5 });
            _mapperMock
                .Setup(mapper => mapper.Map<TournamentParticipant>(participantDto))
                .Returns(participant);

            // Act
            await _participantService.AddAsync(participantDto);

            // Assert
            _participantRepositoryMock.Verify(repo => repo.AddAsync(participant), Times.Once);
        }

        [TestMethod]
        public async Task AddAsync_ShouldThrowException_WhenMaxLimitReached()
        {
            // Arrange
            var tournamentId = Guid.NewGuid();
            var participantDto = new TournamentParticipantCoreDto { TournamentId = tournamentId };

            _participantRepositoryMock
                .Setup(repo => repo.CountParticipantsAsync(tournamentId))
                .ReturnsAsync(5);
            _participantRepositoryMock
                .Setup(repo => repo.GetTournamentAsync(tournamentId))
                .ReturnsAsync(new Tournament { Id = tournamentId, MaxParticipants = 5 });

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _participantService.AddAsync(participantDto)
            );
            _participantRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<TournamentParticipant>()),
                Times.Never
            );
        }
    }
}
