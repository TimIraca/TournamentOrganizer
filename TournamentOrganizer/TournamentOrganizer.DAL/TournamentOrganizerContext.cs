using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.DAL.Entities;

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
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Configurations.TournamentConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PrizeDistributionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.MatchConfiguration());
        }
    }
}
