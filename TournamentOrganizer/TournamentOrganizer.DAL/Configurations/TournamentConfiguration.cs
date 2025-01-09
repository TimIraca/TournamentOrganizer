using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Configurations
{
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder
                .HasMany(t => t.Rounds)
                .WithOne(r => r.Tournament)
                .HasForeignKey(r => r.TournamentId);

            builder
                .HasMany(t => t.Participants)
                .WithOne(p => p.Tournament)
                .HasForeignKey(p => p.TournamentId);
            builder
                .HasOne(t => t.User)
                .WithMany(u => u.Tournaments)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
