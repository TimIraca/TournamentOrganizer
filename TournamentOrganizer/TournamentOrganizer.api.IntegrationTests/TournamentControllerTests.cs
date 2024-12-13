using System.Net;
using System.Net.Http.Json;
using TournamentOrganizer.api.DTOs;

namespace TournamentOrganizer.api.IntegrationTests;

public class TournamentControllerTests
{
    [Fact]
    public async Task CreateTournament_EnsureCreated()
    {
        // arrange
        TournamentOrganizerWebApplicationFactory application = new TournamentOrganizerWebApplicationFactory();
        CreateTournamentApiDto request = new CreateTournamentApiDto
        {
            Name = "Test Tournament",
            StartDate = DateTime.Now,
        };
        HttpClient client = application.CreateClient();
        // act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/tournaments", request);

        // assert
        CreateTournamentApiDto? tournamentResponse = await response.Content.ReadFromJsonAsync<CreateTournamentApiDto>();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(tournamentResponse);
        Assert.Equal(request.Name, tournamentResponse!.Name);
        Assert.Equal(request.StartDate, tournamentResponse.StartDate);
    }
}
