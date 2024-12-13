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
            if (_context.Tournaments.Any())
            {
                Console.WriteLine("Database already seeded.");
                return;
            }

            // Create completed tournament
            Tournament tournament = new Tournament
            {
                Id = Guid.NewGuid(),
                Name = "Completed Tournament",
                StartDate = new DateTime(2024, 12, 1),
                IsCompleted = true,
                Participants = new List<Participant>
                {
                    new Participant { Id = Guid.NewGuid(), Name = "Player 1" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player 2" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player 3" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player 4" },
                },
                Rounds =
                    new List<Round>() // Initialize the Rounds collection
                ,
            };

            // Create rounds and matches
            Round round1 = new Round
            {
                Id = Guid.NewGuid(),
                RoundNumber = 1,
                TournamentId = tournament.Id,
                Matches = new List<Match>
                {
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 1,
                        Participant1Id = tournament.Participants.ElementAt(0).Id,
                        Participant2Id = tournament.Participants.ElementAt(1).Id,
                        WinnerId = tournament
                            .Participants.ElementAt(0)
                            .Id // Player 1 wins
                        ,
                    },
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 2,
                        Participant1Id = tournament.Participants.ElementAt(2).Id,
                        Participant2Id = tournament.Participants.ElementAt(3).Id,
                        WinnerId = tournament
                            .Participants.ElementAt(2)
                            .Id // Player 3 wins
                        ,
                    },
                },
            };

            Round round2 = new Round
            {
                Id = Guid.NewGuid(),
                RoundNumber = 2,
                TournamentId = tournament.Id,
                Matches = new List<Match>
                {
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 1,
                        Participant1Id = tournament.Participants.ElementAt(0).Id,
                        Participant2Id = tournament.Participants.ElementAt(2).Id,
                        WinnerId = tournament
                            .Participants.ElementAt(0)
                            .Id // Player 1 wins final
                        ,
                    },
                },
            };

            // Add rounds to the tournament
            tournament.Rounds.Add(round1);
            tournament.Rounds.Add(round2);

            Tournament incompleteTournament = new Tournament
            {
                Id = Guid.NewGuid(),
                Name = "Incomplete Tournament",
                StartDate = new DateTime(2024, 12, 6),
                IsCompleted = false,
                Participants = new List<Participant>
                {
                    new Participant { Id = Guid.NewGuid(), Name = "Player A" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player B" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player C" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player D" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player E" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player F" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player G" },
                    new Participant { Id = Guid.NewGuid(), Name = "Player H" },
                },
                Rounds =
                    new List<Round>() // Initialize the Rounds collection
                ,
            };

            // Round 1
            Round round1incomplete = new Round
            {
                Id = Guid.NewGuid(),
                RoundNumber = 1,
                TournamentId = incompleteTournament.Id,
                Matches = new List<Match>
                {
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 1,
                        Participant1Id = incompleteTournament.Participants.ElementAt(0).Id,
                        Participant2Id = incompleteTournament.Participants.ElementAt(1).Id,
                        WinnerId = incompleteTournament
                            .Participants.ElementAt(0)
                            .Id // Player A wins
                        ,
                    },
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 2,
                        Participant1Id = incompleteTournament.Participants.ElementAt(2).Id,
                        Participant2Id = incompleteTournament.Participants.ElementAt(3).Id,
                        WinnerId = incompleteTournament
                            .Participants.ElementAt(2)
                            .Id // Player C wins
                        ,
                    },
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 3,
                        Participant1Id = incompleteTournament.Participants.ElementAt(4).Id,
                        Participant2Id = incompleteTournament.Participants.ElementAt(5).Id,
                        WinnerId = incompleteTournament
                            .Participants.ElementAt(4)
                            .Id // Player E wins
                        ,
                    },
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 4,
                        Participant1Id = incompleteTournament.Participants.ElementAt(6).Id,
                        Participant2Id = incompleteTournament.Participants.ElementAt(7).Id,
                        WinnerId = incompleteTournament
                            .Participants.ElementAt(6)
                            .Id // Player G wins
                        ,
                    },
                },
            };

            // Round 2
            Round round2incomplete = new Round
            {
                Id = Guid.NewGuid(),
                RoundNumber = 2,
                TournamentId = incompleteTournament.Id,
                Matches = new List<Match>
                {
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 1,
                        Participant1Id = incompleteTournament.Participants.ElementAt(0).Id,
                        Participant2Id = incompleteTournament.Participants.ElementAt(2).Id,
                        WinnerId = incompleteTournament
                            .Participants.ElementAt(0)
                            .Id // Player A wins
                        ,
                    },
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 2,
                        Participant1Id = incompleteTournament.Participants.ElementAt(4).Id,
                        Participant2Id = incompleteTournament.Participants.ElementAt(6).Id,
                        WinnerId =
                            null // No winner yet
                        ,
                    },
                },
            };

            // Round 3
            Round round3 = new Round
            {
                Id = Guid.NewGuid(),
                RoundNumber = 3,
                TournamentId = incompleteTournament.Id,
                Matches = new List<Match>
                {
                    new Match
                    {
                        Id = Guid.NewGuid(),
                        MatchNumber = 1,
                        Participant1Id = incompleteTournament.Participants.ElementAt(0).Id, // Player A from Round 2
                        Participant2Id = null, // Undefined participant
                        WinnerId =
                            null // Match not played yet
                        ,
                    },
                },
            };

            // Add rounds to the incomplete tournament
            incompleteTournament.Rounds.Add(round1incomplete);
            incompleteTournament.Rounds.Add(round2incomplete);
            incompleteTournament.Rounds.Add(round3);

            // Save data to the database
            _context.Tournaments.Add(tournament);
            _context.Tournaments.Add(incompleteTournament);
            await _context.SaveChangesAsync();

            Console.WriteLine("Database seeded successfully with a completed tournament.");
        }
    }
}
