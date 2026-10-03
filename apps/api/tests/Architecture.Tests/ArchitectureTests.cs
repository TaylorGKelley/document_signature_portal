using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace DocumentSignaturePortal.Architecture.Tests;

public class ArchitectureTests
{
    // Use any existing class/struct/interface from that project:
    private static readonly Assembly DomainAssembly = typeof(DocumentSignaturePortal.Domain.Class1).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(DocumentSignaturePortal.Application.Class1).Assembly;

    [Fact]
    public void Domain_ShouldNotHaveDependencyOnOtherProjects()
    {
        var otherNamespaces = new[]
        {
            "DocumentSignaturePortal.Application",
            "DocumentSignaturePortal.Infrastructure",
            "DocumentSignaturePortal.Api"
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
            .HaveDependencyOn("DocumentSignaturePortal.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, "Application layer must not depend on Infrastructure.");
    }
}
