using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasOne(m => m.Participant1)
                .WithMany()
                .HasForeignKey(m => m.Participant1Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(m => m.Participant2)
                .WithMany()
                .HasForeignKey(m => m.Participant2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
