using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.DAL
{
    public class TournamentContext : DbContext
    {
        public TournamentContext(DbContextOptions<TournamentContext> options)
            : base(options) { }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentParticipant> TournamentParticipants { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PrizeDistribution> PrizeDistributions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>().Property(t => t.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Tournament>().Property(t => t.PrizePool).HasPrecision(18, 2);

            modelBuilder.Entity<PrizeDistribution>().Property(p => p.Percentage).HasPrecision(5, 2);

            modelBuilder
                .Entity<Match>()
                .HasOne(m => m.Participant1)
                .WithMany()
                .HasForeignKey(m => m.Participant1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Match>()
                .HasOne(m => m.Participant2)
                .WithMany()
                .HasForeignKey(m => m.Participant2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
