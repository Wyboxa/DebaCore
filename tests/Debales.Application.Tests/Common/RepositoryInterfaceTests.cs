using Debales.Application.Common;
using Debales.Domain.Common;

namespace Debales.Application.Tests.Common;

// Verifica que IRepository está correctamente definido y puede ser referenciado
public sealed class RepositoryInterfaceTests
{
    [Fact]
    public void IRepository_CanBeUsed_AsGenericConstraint()
    {
        var type = typeof(IRepository<>);

        Assert.True(type.IsInterface);
        Assert.True(type.IsGenericTypeDefinition);
    }

    [Fact]
    public void IUnitOfWork_IsInterface()
    {
        Assert.True(typeof(IUnitOfWork).IsInterface);
    }
}
