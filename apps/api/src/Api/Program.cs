using DocSign.Infrastructure;

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

        // Add services to the container.

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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
