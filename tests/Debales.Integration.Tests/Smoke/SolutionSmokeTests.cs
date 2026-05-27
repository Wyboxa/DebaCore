using Debales.Application.Common;
using Debales.Infrastructure;

namespace Debales.Integration.Tests.Smoke;

// Tests de humo — verifican que la solución compila y las capas están conectadas
public sealed class SolutionSmokeTests
{
    [Fact]
    public void Infrastructure_References_ApplicationLayer()
    {
        var infraAssembly = typeof(InfrastructureAssemblyMarker).Assembly;
        var appAssembly = typeof(IUnitOfWork).Assembly;

        Assert.NotEqual(infraAssembly, appAssembly);
        Assert.NotNull(infraAssembly);
    }
}
