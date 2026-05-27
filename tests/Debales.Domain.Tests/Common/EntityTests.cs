using Debales.Domain.Common;

namespace Debales.Domain.Tests.Common;

public sealed class EntityTests
{
    private sealed class TestEntity : Entity
    {
        public TestEntity(string createdBy)
        {
            CreatedBy = createdBy;
        }

        public void Touch(string updatedBy) => SetUpdated(updatedBy);
    }

    [Fact]
    public void Entity_CreatedAt_IsSet_OnConstruction()
    {
        var entity = new TestEntity("carlos");

        Assert.NotEqual(default, entity.CreatedAt);
        Assert.Equal("carlos", entity.CreatedBy);
    }

    [Fact]
    public void Entity_Id_IsUnique_PerInstance()
    {
        var a = new TestEntity("carlos");
        var b = new TestEntity("carlos");

        Assert.NotEqual(a.Id, b.Id);
    }

    [Fact]
    public void Entity_SetUpdated_SetsUpdatedAt()
    {
        var entity = new TestEntity("carlos");
        entity.Touch("carlos");

        Assert.NotNull(entity.UpdatedAt);
        Assert.Equal("carlos", entity.UpdatedBy);
    }
}
