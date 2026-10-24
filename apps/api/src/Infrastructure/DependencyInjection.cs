using DocSign.Application.Interfaces.Repositories;
using DocSign.Infrastructure.Configuration;
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
        services.ApplyConfigurationOptions();

        services.AddDbContext<Context>(options =>
                    options.UseNpgsql(configuration.BuildConnectionString()));

        // Repositories
        services.AddScoped<IDocumentRepository, DocumentRepository>();

        return services;
    }

    private static IServiceCollection ApplyConfigurationOptions(this IServiceCollection services)
    {
        services.AddOptions<StorageOptions>()
            .BindConfiguration(StorageOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(RabbitMqOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<Context>();

        return services;
    }

    public static IHealthChecksBuilder AddDbContextCheck(this IHealthChecksBuilder check, string name, string[] tags)
    {
        check.AddDbContextCheck<Context>(name, tags: tags);

        return check;
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
            Host = configuration["DATABASE__POSTGRES__HOST"] ?? "localhost",
            Port = int.Parse(configuration["DATABASE__POSTGRES__PORT"] ?? "5432"),
            Database = configuration["DATABASE__POSTGRES__DB"] ?? "document_signature_portal",
            Username = configuration["DATABASE__POSTGRES__USER"] ?? "postgres",
            Password = configuration["DATABASE__POSTGRES__PASSWORD"],
            SslMode = SslMode.Prefer,
            Pooling = true,
            MaxPoolSize = 100
        };

        return builder.ConnectionString;
    }
}
