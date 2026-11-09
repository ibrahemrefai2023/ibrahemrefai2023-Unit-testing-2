using System.Reflection;
using FluentAssertions;
using ParkSmart.Application.Services;
using ParkSmart.Domain;

namespace ParkSmart.ArchitectureTests;

public class ProjectDependencyTests
{
    [Fact]
    public void Domain_HasNoParkSmartProjectReferences()
    {
        var dependencies = typeof(ParkingSession).Assembly.GetReferencedAssemblies();

        dependencies.Should().NotContain(reference => reference.Name!.StartsWith("ParkSmart."));
    }

    [Fact]
    public void Application_ReferencesOnlyTheDomainProject()
    {
        var dependencies = ProjectDependencies(typeof(ParkingFeeCalculator).Assembly);

        dependencies.Should().ContainSingle().Which.Should().Be("ParkSmart.Domain");
    }

    [Fact]
    public void ParkingFeeCalculator_UsesTheExpectedCalculatorName()
    {
        typeof(ParkingFeeCalculator).Name.Should().Be("ParkingFeeCalculator");
        typeof(ParkingFeeCalculator).Namespace.Should().Be("ParkSmart.Application.Services");
    }

    private static IEnumerable<string> ProjectDependencies(Assembly assembly) =>
        assembly.GetReferencedAssemblies()
            .Select(reference => reference.Name!)
            .Where(name => name.StartsWith("ParkSmart."));
}
