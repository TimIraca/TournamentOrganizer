using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Configurations
{
    public class RoundConfiguration : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> builder)
        {
            builder.HasMany(r => r.Matches).WithOne(m => m.Round).HasForeignKey(m => m.RoundId);
        }
    }
}
