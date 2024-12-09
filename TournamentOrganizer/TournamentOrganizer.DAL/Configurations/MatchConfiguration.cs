using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            // Configure Participant1 relationship
            builder
                .HasOne(m => m.Participant1)
                .WithMany(p => p.MatchesAsParticipant1)
                .HasForeignKey(m => m.Participant1Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Participant2 relationship
            builder
                .HasOne(m => m.Participant2)
                .WithMany(p => p.MatchesAsParticipant2)
                .HasForeignKey(m => m.Participant2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Winner relationship (if needed)
            builder
                .HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
