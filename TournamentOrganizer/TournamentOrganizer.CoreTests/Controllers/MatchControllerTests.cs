using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TournamentOrganizer.api.Controllers;
using TournamentOrganizer.api.DTOs;
using TournamentOrganizer.Core;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

namespace TournamentOrganizer.api.Controllers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    namespace TournamentOrganizer.Tests.Controllers
    {
        [TestClass]
        public class MatchControllerTests
        {
            private Mock<IMatchService> _mockMatchService;
            private Mock<IMapper> _mockMapper;
            private MatchController _controller;

            [TestInitialize]
            public void Setup()
            {
                _mockMatchService = new Mock<IMatchService>();
                _mockMapper = new Mock<IMapper>();
                _controller = new MatchController(_mockMatchService.Object, _mockMapper.Object);
            }

            [TestMethod]
            public async Task GetMatchesByRoundId_ReturnsOkResult_WithMappedMatches()
            {
                // Arrange
                Guid roundId = Guid.NewGuid();
                List<MatchCoreDto> coreDtos = new List<MatchCoreDto>
                {
                    new() { Id = Guid.NewGuid() },
                };
                List<MatchApiDto> apiDtos = new List<MatchApiDto> { new() { Id = coreDtos[0].Id } };

                _mockMatchService
                    .Setup(s => s.GetAllByRoundIdAsync(roundId))
                    .ReturnsAsync(coreDtos);
                _mockMapper.Setup(m => m.Map<IEnumerable<MatchApiDto>>(coreDtos)).Returns(apiDtos);

                // Act
                IActionResult result = await _controller.GetMatchesByRoundId(roundId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                OkObjectResult okResult = (OkObjectResult)result;
                IEnumerable<MatchApiDto>? returnedDtos = (IEnumerable<MatchApiDto>)okResult.Value;
                CollectionAssert.AreEqual(apiDtos, new List<MatchApiDto>(returnedDtos));
            }

            [TestMethod]
            public async Task GetMatchesByRoundId_ReturnsServerError_WhenExceptionOccurs()
            {
                // Arrange
                Guid roundId = Guid.NewGuid();
                _mockMatchService
                    .Setup(s => s.GetAllByRoundIdAsync(roundId))
                    .ThrowsAsync(new Exception("Test error"));

                // Act
                IActionResult result = await _controller.GetMatchesByRoundId(roundId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ObjectResult));
                ObjectResult statusResult = (ObjectResult)result;
                Assert.AreEqual(500, statusResult.StatusCode);
                Assert.AreEqual("Test error", statusResult.Value);
            }

            [TestMethod]
            public async Task GetMatchById_ReturnsOkResult_WhenMatchExists()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();
                MatchCoreDto coreDto = new MatchCoreDto { Id = matchId };
                MatchApiDto apiDto = new MatchApiDto { Id = matchId };

                _mockMatchService.Setup(s => s.GetByIdAsync(matchId)).ReturnsAsync(coreDto);
                _mockMapper.Setup(m => m.Map<MatchApiDto>(coreDto)).Returns(apiDto);

                // Act
                IActionResult result = await _controller.GetMatchById(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                OkObjectResult okResult = (OkObjectResult)result;
                Assert.AreEqual(apiDto, okResult.Value);
            }

            [TestMethod]
            public async Task GetMatchById_ReturnsNotFound_WhenMatchDoesNotExist()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();
                _mockMatchService
                    .Setup(s => s.GetByIdAsync(matchId))
                    .ThrowsAsync(new NotFoundException($"Match with ID {matchId} not found"));

                // Act
                IActionResult result = await _controller.GetMatchById(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
                NotFoundObjectResult notFoundResult = (NotFoundObjectResult)result;
                Assert.AreEqual($"Match with ID {matchId} not found", notFoundResult.Value);
            }

            [TestMethod]
            public async Task AddMatch_ReturnsCreatedAtAction_WhenSuccessful()
            {
                // Arrange
                MatchApiDto apiDto = new MatchApiDto { Id = Guid.NewGuid() };
                MatchCoreDto coreDto = new MatchCoreDto { Id = apiDto.Id };

                _mockMapper.Setup(m => m.Map<MatchCoreDto>(apiDto)).Returns(coreDto);
                _mockMatchService.Setup(s => s.AddAsync(coreDto)).ReturnsAsync(coreDto);
                _mockMapper.Setup(m => m.Map<MatchApiDto>(coreDto)).Returns(apiDto);

                // Act
                IActionResult result = await _controller.AddMatch(apiDto);

                // Assert
                Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
                CreatedAtActionResult createdResult = (CreatedAtActionResult)result;
                Assert.AreEqual(nameof(MatchController.GetMatchById), createdResult.ActionName);
                Assert.AreEqual(apiDto.Id, ((dynamic)createdResult.RouteValues!)["id"]);
                Assert.AreEqual(apiDto, createdResult.Value);
            }

            [TestMethod]
            public async Task UpdateMatch_ReturnsNoContent_WhenSuccessful()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();
                MatchApiDto apiDto = new MatchApiDto { Id = matchId };
                MatchCoreDto coreDto = new MatchCoreDto { Id = matchId };

                _mockMapper.Setup(m => m.Map<MatchCoreDto>(apiDto)).Returns(coreDto);

                // Act
                IActionResult result = await _controller.UpdateMatch(matchId, apiDto);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }

            [TestMethod]
            public async Task UpdateMatch_ReturnsBadRequest_WhenIdMismatch()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();
                Guid differentId = Guid.NewGuid();
                MatchApiDto apiDto = new MatchApiDto { Id = differentId };

                // Act
                IActionResult result = await _controller.UpdateMatch(matchId, apiDto);

                // Assert
                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                BadRequestObjectResult badRequestResult = (BadRequestObjectResult)result;
                Assert.AreEqual("ID mismatch between URL and body", badRequestResult.Value);
            }

            [TestMethod]
            public async Task DeclareWinner_ReturnsOk_WhenSuccessful()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();
                Guid tournamentId = Guid.NewGuid();
                Guid winnerId = Guid.NewGuid();
                DeclareWinnerRequestDto request = new DeclareWinnerRequestDto
                {
                    WinnerId = winnerId,
                };

                // Act
                IActionResult result = await _controller.DeclareWinner(
                    matchId,
                    tournamentId,
                    request
                );

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkResult));
            }

            [TestMethod]
            public async Task DeclareWinner_ReturnsBadRequest_WhenPlayerNotInMatch()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();
                Guid tournamentId = Guid.NewGuid();
                Guid winnerId = Guid.NewGuid();
                DeclareWinnerRequestDto request = new DeclareWinnerRequestDto
                {
                    WinnerId = winnerId,
                };

                _mockMatchService
                    .Setup(s => s.DeclareMatchWinnerAsync(tournamentId, matchId, winnerId))
                    .ThrowsAsync(new InvalidOperationException("Player is not a participant"));

                // Act
                IActionResult result = await _controller.DeclareWinner(
                    matchId,
                    tournamentId,
                    request
                );

                // Assert
                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                BadRequestObjectResult badRequestResult = (BadRequestObjectResult)result;
                Assert.AreEqual("Player is not a participant", badRequestResult.Value);
            }

            [TestMethod]
            public async Task DeleteMatch_ReturnsNoContent_WhenSuccessful()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();

                // Act
                IActionResult result = await _controller.DeleteMatch(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }

            [TestMethod]
            public async Task DeleteMatch_ReturnsNotFound_WhenMatchDoesNotExist()
            {
                // Arrange
                Guid matchId = Guid.NewGuid();
                _mockMatchService
                    .Setup(s => s.DeleteAsync(matchId))
                    .ThrowsAsync(new NotFoundException($"Match with ID {matchId} not found"));

                // Act
                IActionResult result = await _controller.DeleteMatch(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
                NotFoundObjectResult notFoundResult = (NotFoundObjectResult)result;
                Assert.AreEqual($"Match with ID {matchId} not found", notFoundResult.Value);
            }
        }
    }
}
