using DocSign.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace DocSign.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
                .AddEnvironmentVariables()
                .AddCommandLine(args).Build();

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddIdentity();

        builder.Services.AddHealthChecks()
            .AddDbContextCheck(
                name: "database_check",
                tags: ["ready"]);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapIdentityApi();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            await app.ApplyMigrations();
            app.MapOpenApi();
        }

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false // Exclude all checks and return true when app is running
        });
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
