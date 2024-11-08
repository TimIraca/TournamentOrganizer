using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.api.Mappings;
using TournamentOrganizer.Core.Mappings;
using TournamentOrganizer.Core.Services.Implementations;
using TournamentOrganizer.Core.Services.Interfaces;
using TournamentOrganizer.DAL;
using TournamentOrganizer.DAL.Repositories.Implementations;
using TournamentOrganizer.DAL.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactApp",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TournamentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(ApiMappingProfile));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TournamentContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Attempting to ensure database exists and is up to date");

        // This will create the database if it doesn't exist
        context.Database.EnsureCreated();

        // Apply any pending migrations
        if (context.Database.GetPendingMigrations().Any())
        {
            logger.LogInformation("Applying pending migrations");
            context.Database.Migrate();
        }

        logger.LogInformation("Database setup completed successfully");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while setting up the database.");
        throw; // Rethrow to prevent application startup if database setup fails
    }
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
