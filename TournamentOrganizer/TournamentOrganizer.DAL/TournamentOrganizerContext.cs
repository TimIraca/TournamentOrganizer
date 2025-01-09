using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL
{
    public class TournamentContext : DbContext
    {
        public TournamentContext(DbContextOptions<TournamentContext> options)
            : base(options) { }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Configurations.TournamentConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RoundConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.MatchConfiguration());
        }
    }
}
