using DocSign.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DocSign.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
