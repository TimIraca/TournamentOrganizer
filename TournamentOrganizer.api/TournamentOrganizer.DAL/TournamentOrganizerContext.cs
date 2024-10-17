using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.DAL
{
    public class TournamentOrganizerContext : DbContext
    {
        public TournamentOrganizerContext(DbContextOptions<TournamentOrganizerContext> options)
            : base(options) { }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Tournament>()
                .HasData(
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

            modelBuilder
                .Entity<Player>()
                .HasData(
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
    }
}
