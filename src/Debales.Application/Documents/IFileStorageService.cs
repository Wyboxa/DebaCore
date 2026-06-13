namespace Debales.Application.Documents;

public interface IFileStorageService
{
    Task<string> SaveAsync(string fileName, Stream content, CancellationToken ct = default);
    Task DeleteAsync(string storedName, CancellationToken ct = default);
}
