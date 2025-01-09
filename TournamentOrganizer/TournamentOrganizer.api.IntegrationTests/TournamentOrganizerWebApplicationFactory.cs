using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TournamentOrganizer.DAL;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.api.IntegrationTests
{
    internal class TournamentOrganizerWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<TournamentContext>>();
                services.AddDbContext<TournamentContext>(options =>
                {
                    options.UseSqlServer(
                        "Server=localhost,1433;Database=TournamentDb_Test;User=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
                    );
                });
                services
                    .AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme",
                        options => { }
                    );

                TournamentContext dbContext = CreateDbContext(services);
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
                var testUser = new User
                {
                    Id = new Guid("2fd1c8d5-ba31-4dbf-9352-d6208fd89763"), // Must match the ID in TestAuthHandler
                    Username = "test",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("test"),
                };

                dbContext.Users.Add(testUser);
                dbContext.SaveChanges();
            });
        }

        private static TournamentContext CreateDbContext(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IServiceScope scope = serviceProvider.CreateScope();
            TournamentContext dbContext =
                scope.ServiceProvider.GetRequiredService<TournamentContext>();
            return dbContext;
        }
    }
}
