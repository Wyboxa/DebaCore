namespace Debales.Application.Common;

public interface ITenantService
{
    Guid? CurrentTenantId { get; }
}
