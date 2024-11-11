using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL
{
    public class DatabaseSeeder
    {
        private readonly TournamentContext _context;

        public DatabaseSeeder(TournamentContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Only seed if there are no tournaments
            if (!_context.Tournaments.Any())
            {
                var tournaments = new List<Tournament>
                {
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Name = "Summer Chess Championship",
                        Description = "Annual chess tournament for all skill levels",
                        Format = TournamentFormat.SingleElimination,
                        Status = TournamentStatus.Registration,
                        StartDate = DateTime.UtcNow.AddDays(7),
                        MaxParticipants = 16,
                        PrizePool = 1000m,
                        PrizeCurrency = "USD",
                    },
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Name = "Weekly Poker Tournament",
                        Description = "Weekly poker tournament with rotating formats",
                        Format = TournamentFormat.SingleElimination,
                        Status = TournamentStatus.Registration,
                        StartDate = DateTime.UtcNow.AddDays(1),
                        MaxParticipants = 8,
                        PrizePool = 500m,
                        PrizeCurrency = "USD",
                    },
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Name = "League of Legends Round Robin",
                        Description = "Monthly LoL tournament with round-robin format",
                        Format = TournamentFormat.RoundRobin,
                        Status = TournamentStatus.Registration,
                        StartDate = DateTime.UtcNow.AddDays(14),
                        MaxParticipants = 8,
                        PrizePool = 2000m,
                        PrizeCurrency = "USD",
                    },
                };

                // Add prize distributions
                foreach (var tournament in tournaments)
                {
                    tournament.PrizeDistributions.Add(
                        new PrizeDistribution
                        {
                            Id = Guid.NewGuid(),
                            Place = 1,
                            Percentage = 60m,
                        }
                    );
                    tournament.PrizeDistributions.Add(
                        new PrizeDistribution
                        {
                            Id = Guid.NewGuid(),
                            Place = 2,
                            Percentage = 30m,
                        }
                    );
                    tournament.PrizeDistributions.Add(
                        new PrizeDistribution
                        {
                            Id = Guid.NewGuid(),
                            Place = 3,
                            Percentage = 10m,
                        }
                    );
                }

                // Add some participants to each tournament
                var random = new Random();
                foreach (var tournament in tournaments)
                {
                    var participantCount = random.Next(2, 5); // Random number of initial participants
                    for (int i = 1; i <= participantCount; i++)
                    {
                        var participant = new TournamentParticipant
                        {
                            Id = Guid.NewGuid(),
                            TournamentId = tournament.Id,
                            ParticipantName = $"Player {i} - {tournament.Name}",
                            RegistrationDate = DateTime.UtcNow.AddDays(-random.Next(1, 5)),
                        };
                        tournament.Participants.Add(participant);
                    }
                }

                await _context.Tournaments.AddRangeAsync(tournaments);

                // Create a completed tournament
                var completedTournament = new Tournament
                {
                    Id = Guid.NewGuid(),
                    Name = "Last Month's Chess Tournament",
                    Description = "Completed chess tournament from last month",
                    Format = TournamentFormat.SingleElimination,
                    Status = TournamentStatus.Completed,
                    StartDate = DateTime.UtcNow.AddMonths(-1),
                    EndDate = DateTime.UtcNow.AddMonths(-1).AddDays(2),
                    MaxParticipants = 4,
                    PrizePool = 400m,
                    PrizeCurrency = "USD",
                };
                var completedTournament2 = new Tournament
                {
                    Id = Guid.NewGuid(),
                    Name = "Last Month's Chess Tournament",
                    Description = "Completed chess tournament from last month",
                    Format = TournamentFormat.SingleElimination,
                    Status = TournamentStatus.Completed,
                    StartDate = DateTime.UtcNow.AddMonths(-1),
                    EndDate = DateTime.UtcNow.AddMonths(-1).AddDays(2),
                    MaxParticipants = 4,
                    PrizePool = 400m,
                    PrizeCurrency = "USD",
                };

                // Add prize distributions for completed tournament
                completedTournament.PrizeDistributions.Add(
                    new PrizeDistribution
                    {
                        Id = Guid.NewGuid(),
                        Place = 1,
                        Percentage = 70m,
                    }
                );
                completedTournament.PrizeDistributions.Add(
                    new PrizeDistribution
                    {
                        Id = Guid.NewGuid(),
                        Place = 2,
                        Percentage = 30m,
                    }
                );

                // Add participants for completed tournament
                for (int i = 1; i <= 4; i++)
                {
                    var participant = new TournamentParticipant
                    {
                        Id = Guid.NewGuid(),
                        TournamentId = completedTournament.Id,
                        ParticipantName = $"Historical Player {i}",
                        RegistrationDate = DateTime.UtcNow.AddMonths(-1).AddDays(-2),
                    };
                    completedTournament.Participants.Add(participant);
                }

                // Create completed matches
                var participants = completedTournament.Participants.ToList();

                // Semi-finals
                completedTournament.Matches.Add(
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        TournamentId = completedTournament.Id,
                        Round = 1,
                        Participant1Id = participants[0].Id,
                        Participant2Id = participants[1].Id,
                        Participant1Score = 2,
                        Participant2Score = 1,
                        Status = MatchStatus.Completed,
                        WinnerId = participants[0].Id,
                        ScheduledTime = completedTournament.StartDate,
                    }
                );

                completedTournament.Matches.Add(
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        TournamentId = completedTournament.Id,
                        Round = 1,
                        Participant1Id = participants[2].Id,
                        Participant2Id = participants[3].Id,
                        Participant1Score = 0,
                        Participant2Score = 2,
                        Status = MatchStatus.Completed,
                        WinnerId = participants[3].Id,
                        ScheduledTime = completedTournament.StartDate,
                    }
                );

                // Finals
                completedTournament.Matches.Add(
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        TournamentId = completedTournament.Id,
                        Round = 2,
                        Participant1Id = participants[0].Id,
                        Participant2Id = participants[3].Id,
                        Participant1Score = 3,
                        Participant2Score = 1,
                        Status = MatchStatus.Completed,
                        WinnerId = participants[0].Id,
                        ScheduledTime = completedTournament.StartDate.AddDays(1),
                    }
                );

                await _context.Tournaments.AddAsync(completedTournament);
                await _context.SaveChangesAsync();
            }
        }
    }
}
