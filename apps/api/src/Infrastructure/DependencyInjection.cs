using DocSign.Application.Interfaces.Repositories;
using DocSign.Infrastructure.Persistence;
using DocSign.Infrastructure.Persistence.Identity;
using DocSign.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DocSign.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
                    options.UseNpgsql(configuration.BuildConnectionString()));

        // Repositories
        services.AddScoped<IDocumentRepository, DocumentRepository>();

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<Context>();

        return services;
    }

    public static IEndpointRouteBuilder MapIdentityApi(this IEndpointRouteBuilder app)
    {
        app.MapIdentityApi<User>();

        return app;
    }

    public static async Task<IEndpointRouteBuilder> ApplyMigrations(this IEndpointRouteBuilder app)
    {
        using var scope = app.ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<Context>();
        await context.Database.MigrateAsync();

        return app;
    }

    private static string BuildConnectionString(this IConfiguration configuration)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = configuration["POSTGRES_HOST"] ?? "localhost",
            Port = int.Parse(configuration["POSTGRES_PORT"] ?? "5432"),
            Database = configuration["POSTGRES_DB"] ?? "document_signature_portal",
            Username = configuration["POSTGRES_USER"] ?? "postgres",
            Password = configuration["POSTGRES_PASSWORD"],
            SslMode = SslMode.Prefer,
            Pooling = true,
            MaxPoolSize = 100
        };

        return builder.ConnectionString;
    }
}
