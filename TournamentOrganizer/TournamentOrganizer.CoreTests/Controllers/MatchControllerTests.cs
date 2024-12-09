﻿using System;
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
                var roundId = Guid.NewGuid();
                var coreDtos = new List<MatchCoreDto> { new() { Id = Guid.NewGuid() } };
                var apiDtos = new List<MatchApiDto> { new() { Id = coreDtos[0].Id } };

                _mockMatchService
                    .Setup(s => s.GetAllByRoundIdAsync(roundId))
                    .ReturnsAsync(coreDtos);
                _mockMapper.Setup(m => m.Map<IEnumerable<MatchApiDto>>(coreDtos)).Returns(apiDtos);

                // Act
                var result = await _controller.GetMatchesByRoundId(roundId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                var okResult = (OkObjectResult)result;
                var returnedDtos = (IEnumerable<MatchApiDto>)okResult.Value;
                CollectionAssert.AreEqual(apiDtos, new List<MatchApiDto>(returnedDtos));
            }

            [TestMethod]
            public async Task GetMatchesByRoundId_ReturnsServerError_WhenExceptionOccurs()
            {
                // Arrange
                var roundId = Guid.NewGuid();
                _mockMatchService
                    .Setup(s => s.GetAllByRoundIdAsync(roundId))
                    .ThrowsAsync(new Exception("Test error"));

                // Act
                var result = await _controller.GetMatchesByRoundId(roundId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ObjectResult));
                var statusResult = (ObjectResult)result;
                Assert.AreEqual(500, statusResult.StatusCode);
                Assert.AreEqual("Test error", statusResult.Value);
            }

            [TestMethod]
            public async Task GetMatchById_ReturnsOkResult_WhenMatchExists()
            {
                // Arrange
                var matchId = Guid.NewGuid();
                var coreDto = new MatchCoreDto { Id = matchId };
                var apiDto = new MatchApiDto { Id = matchId };

                _mockMatchService.Setup(s => s.GetByIdAsync(matchId)).ReturnsAsync(coreDto);
                _mockMapper.Setup(m => m.Map<MatchApiDto>(coreDto)).Returns(apiDto);

                // Act
                var result = await _controller.GetMatchById(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                var okResult = (OkObjectResult)result;
                Assert.AreEqual(apiDto, okResult.Value);
            }

            [TestMethod]
            public async Task GetMatchById_ReturnsNotFound_WhenMatchDoesNotExist()
            {
                // Arrange
                var matchId = Guid.NewGuid();
                _mockMatchService
                    .Setup(s => s.GetByIdAsync(matchId))
                    .ThrowsAsync(new NotFoundException($"Match with ID {matchId} not found"));

                // Act
                var result = await _controller.GetMatchById(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
                var notFoundResult = (NotFoundObjectResult)result;
                Assert.AreEqual($"Match with ID {matchId} not found", notFoundResult.Value);
            }

            [TestMethod]
            public async Task AddMatch_ReturnsCreatedAtAction_WhenSuccessful()
            {
                // Arrange
                var apiDto = new MatchApiDto { Id = Guid.NewGuid() };
                var coreDto = new MatchCoreDto { Id = apiDto.Id };

                _mockMapper.Setup(m => m.Map<MatchCoreDto>(apiDto)).Returns(coreDto);
                _mockMatchService.Setup(s => s.AddAsync(coreDto)).ReturnsAsync(coreDto);
                _mockMapper.Setup(m => m.Map<MatchApiDto>(coreDto)).Returns(apiDto);

                // Act
                var result = await _controller.AddMatch(apiDto);

                // Assert
                Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
                var createdResult = (CreatedAtActionResult)result;
                Assert.AreEqual(nameof(MatchController.GetMatchById), createdResult.ActionName);
                Assert.AreEqual(apiDto.Id, ((dynamic)createdResult.RouteValues!)["id"]);
                Assert.AreEqual(apiDto, createdResult.Value);
            }

            [TestMethod]
            public async Task UpdateMatch_ReturnsNoContent_WhenSuccessful()
            {
                // Arrange
                var matchId = Guid.NewGuid();
                var apiDto = new MatchApiDto { Id = matchId };
                var coreDto = new MatchCoreDto { Id = matchId };

                _mockMapper.Setup(m => m.Map<MatchCoreDto>(apiDto)).Returns(coreDto);

                // Act
                var result = await _controller.UpdateMatch(matchId, apiDto);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }

            [TestMethod]
            public async Task UpdateMatch_ReturnsBadRequest_WhenIdMismatch()
            {
                // Arrange
                var matchId = Guid.NewGuid();
                var differentId = Guid.NewGuid();
                var apiDto = new MatchApiDto { Id = differentId };

                // Act
                var result = await _controller.UpdateMatch(matchId, apiDto);

                // Assert
                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                var badRequestResult = (BadRequestObjectResult)result;
                Assert.AreEqual("ID mismatch between URL and body", badRequestResult.Value);
            }

            [TestMethod]
            public async Task DeclareWinner_ReturnsOk_WhenSuccessful()
            {
                // Arrange
                var matchId = Guid.NewGuid();
                var tournamentId = Guid.NewGuid();
                var winnerId = Guid.NewGuid();
                var request = new DeclareWinnerRequestDto { WinnerId = winnerId };

                // Act
                var result = await _controller.DeclareWinner(matchId, tournamentId, request);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkResult));
            }

            [TestMethod]
            public async Task DeclareWinner_ReturnsBadRequest_WhenPlayerNotInMatch()
            {
                // Arrange
                var matchId = Guid.NewGuid();
                var tournamentId = Guid.NewGuid();
                var winnerId = Guid.NewGuid();
                var request = new DeclareWinnerRequestDto { WinnerId = winnerId };

                _mockMatchService
                    .Setup(s => s.DeclareMatchWinnerAsync(tournamentId, matchId, winnerId))
                    .ThrowsAsync(new InvalidOperationException("Player is not a participant"));

                // Act
                var result = await _controller.DeclareWinner(matchId, tournamentId, request);

                // Assert
                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
                var badRequestResult = (BadRequestObjectResult)result;
                Assert.AreEqual("Player is not a participant", badRequestResult.Value);
            }

            [TestMethod]
            public async Task DeleteMatch_ReturnsNoContent_WhenSuccessful()
            {
                // Arrange
                var matchId = Guid.NewGuid();

                // Act
                var result = await _controller.DeleteMatch(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }

            [TestMethod]
            public async Task DeleteMatch_ReturnsNotFound_WhenMatchDoesNotExist()
            {
                // Arrange
                var matchId = Guid.NewGuid();
                _mockMatchService
                    .Setup(s => s.DeleteAsync(matchId))
                    .ThrowsAsync(new NotFoundException($"Match with ID {matchId} not found"));

                // Act
                var result = await _controller.DeleteMatch(matchId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
                var notFoundResult = (NotFoundObjectResult)result;
                Assert.AreEqual($"Match with ID {matchId} not found", notFoundResult.Value);
            }
        }
    }
}