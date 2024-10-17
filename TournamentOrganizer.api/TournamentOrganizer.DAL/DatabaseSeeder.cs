using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.DAL
{
    public class DatabaseSeeder
    {
        private readonly TournamentOrganizerContext _context;

        public DatabaseSeeder(TournamentOrganizerContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Tournaments.Any())
            {
                _context.Tournaments.AddRange(
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Name = "Spring Championship",
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(1).AddDays(7),
                        RankingSystem = RankingSystem.RoundRobin,
                    },
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Name = "Summer Showdown",
                        StartDate = DateTime.Now.AddMonths(3),
                        EndDate = DateTime.Now.AddMonths(3).AddDays(10),
                        RankingSystem = RankingSystem.SingleElimination,
                    }
                );
            }

            if (!_context.Players.Any())
            {
                _context.Players.AddRange(
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Name = "Alice",
                        Ranking = 1200,
                    },
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Name = "Bob",
                        Ranking = 1250,
                    },
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Name = "Charlie",
                        Ranking = 1150,
                    }
                );
            }

            _context.SaveChanges();
        }
    }
}
