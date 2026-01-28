using System.Collections.Concurrent;
using Araponga.Application.Interfaces;

namespace Araponga.Tests.TestHelpers.ExternalServices;

/// <summary>
/// Implementação in-memory de IDistributedCacheService para testes.
/// Suporta reset e configuração de comportamento via ITestExternalService.
/// </summary>
public sealed class InMemoryRedisService : IDistributedCacheService, ITestExternalService
{
    private readonly ConcurrentDictionary<string, (object Value, DateTime? ExpiresAt)> _cache = new();
    private readonly Dictionary<string, object?> _behaviors = new();

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        if (_behaviors.TryGetValue("return_null", out var returnNull) && returnNull != null)
        {
            return Task.FromResult<T?>(default);
        }

        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt.HasValue && entry.ExpiresAt.Value < DateTime.UtcNow)
            {
                _cache.TryRemove(key, out _);
                return Task.FromResult<T?>(default);
            }

            if (entry.Value is T typed)
            {
                return Task.FromResult<T?>(typed);
            }
        }

        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        var expiresAt = expiration == TimeSpan.Zero ? null : (DateTime?)DateTime.UtcNow.Add(expiration);
        _cache.AddOrUpdate(key, (value, expiresAt), (k, v) => (value, expiresAt));
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        _cache.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        var prefix = pattern.EndsWith("*", StringComparison.Ordinal)
            ? pattern[..^1]
            : pattern;

        var toRemove = _cache.Keys.Where(k => k.StartsWith(prefix, StringComparison.Ordinal)).ToList();
        foreach (var k in toRemove)
        {
            _cache.TryRemove(k, out _);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt.HasValue && entry.ExpiresAt.Value < DateTime.UtcNow)
            {
                _cache.TryRemove(key, out _);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public void Reset()
    {
        _cache.Clear();
        _behaviors.Clear();
    }

    public void ConfigureBehavior(string behavior, object? result = null)
    {
        _behaviors[behavior] = result;
    }
}
