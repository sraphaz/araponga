using System.Collections.Concurrent;
using Araponga.Application.Interfaces.Media;

namespace Araponga.Tests.TestHelpers.ExternalServices;

/// <summary>
/// Implementação in-memory de IMediaStorageService para testes.
/// Armazena arquivos em memória e suporta reset e configuração de comportamento.
/// </summary>
public sealed class InMemoryS3Service : IMediaStorageService, ITestExternalService
{
    private readonly ConcurrentDictionary<string, (byte[] Content, string MimeType, string FileName)> _storage = new();
    private readonly Dictionary<string, object?> _behaviors = new();

    public Task<string> UploadAsync(Stream stream, string mimeType, string fileName, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        if (_behaviors.TryGetValue("return_empty_key", out var returnEmpty) && returnEmpty != null)
        {
            return Task.FromResult(string.Empty);
        }

        var storageKey = $"test/{Guid.NewGuid()}/{fileName}";
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        var content = memoryStream.ToArray();

        _storage.AddOrUpdate(storageKey, (content, mimeType, fileName), (k, v) => (content, mimeType, fileName));
        return Task.FromResult(storageKey);
    }

    public Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        if (_behaviors.TryGetValue("throw_not_found", out var throwNotFound) && throwNotFound != null)
        {
            throw new FileNotFoundException($"File not found: {storageKey}");
        }

        if (_storage.TryGetValue(storageKey, out var entry))
        {
            return Task.FromResult<Stream>(new MemoryStream(entry.Content));
        }

        throw new FileNotFoundException($"File not found: {storageKey}");
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        _storage.TryRemove(storageKey, out _);
        return Task.CompletedTask;
    }

    public Task<string> GetUrlAsync(string storageKey, TimeSpan? expiresIn = null, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        if (_storage.TryGetValue(storageKey, out _))
        {
            var url = $"https://test-storage.example.com/{storageKey}";
            return Task.FromResult(url);
        }

        throw new FileNotFoundException($"File not found: {storageKey}");
    }

    public Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_storage.ContainsKey(storageKey));
    }

    public void Reset()
    {
        _storage.Clear();
        _behaviors.Clear();
    }

    public void ConfigureBehavior(string behavior, object? result = null)
    {
        _behaviors[behavior] = result;
    }
}
