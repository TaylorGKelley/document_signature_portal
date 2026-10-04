using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace DocSign.Architecture.Tests;

public class ArchitectureTests
{
    // Use any existing class/struct/interface from that project:
    private static readonly Assembly DomainAssembly = typeof(Domain.Common.DomainException).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;

    [Fact]
    public void Domain_ShouldNotHaveDependencyOnOtherProjects()
    {
        var otherNamespaces = new[]
        {
            "DocSign.Application",
            "DocSign.Infrastructure",
            "DocSign.Api"
        };

        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherNamespaces)
            .GetResult();

        Assert.True(result.IsSuccessful, "Domain layer must not depend on outer layers.");
    }

    [Fact]
    public void Application_ShouldNotDependOnInfrastructure()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOn("DocSign.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, "Application layer must not depend on Infrastructure.");
    }
}
