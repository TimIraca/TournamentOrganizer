using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentOrganizer.DAL.Entities;


namespace TournamentOrganizer.DAL.Configurations
{
    public class PrizeDistributionConfiguration : IEntityTypeConfiguration<PrizeDistribution>
    {
        public void Configure(EntityTypeBuilder<PrizeDistribution> builder)
        {

            builder.Property(p => p.Percentage).HasPrecision(5, 2);
        }
    }
}
