using Debales.Application.Documents;
using Microsoft.Extensions.Configuration;

namespace Debales.Infrastructure.Documents;

public sealed class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"]
            ?? Path.Combine(AppContext.BaseDirectory, "uploads");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(string fileName, Stream content, CancellationToken ct = default)
    {
        var safeName = Path.GetFileName(fileName);
        var uniqueName = $"{Guid.NewGuid():N}_{safeName}";
        var fullPath = Path.Combine(_basePath, uniqueName);
        await using var fs = File.Create(fullPath);
        await content.CopyToAsync(fs, ct);
        return uniqueName;
    }

    public Task DeleteAsync(string storedName, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_basePath, storedName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }
}
