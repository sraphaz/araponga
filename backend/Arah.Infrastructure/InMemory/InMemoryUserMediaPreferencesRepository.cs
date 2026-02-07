using Arah.Application.Interfaces.Users;
using Arah.Domain.Users;

namespace Arah.Infrastructure.InMemory;

/// <summary>
/// Implementação in-memory do repositório de preferências de mídia do usuário.
/// </summary>
public sealed class InMemoryUserMediaPreferencesRepository : IUserMediaPreferencesRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryUserMediaPreferencesRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<UserMediaPreferences?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var preferences = _dataStore.UserMediaPreferences.FirstOrDefault(p => p.UserId == userId);
        return Task.FromResult(preferences);
    }

    public Task<UserMediaPreferences> GetOrCreateDefaultAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var existing = _dataStore.UserMediaPreferences.FirstOrDefault(p => p.UserId == userId);
        if (existing is not null)
        {
            return Task.FromResult(existing);
        }

        // Criar preferências padrão
        var defaultPreferences = new UserMediaPreferences
        {
            UserId = userId,
            ShowImages = true,
            ShowVideos = true,
            ShowAudio = true,
            AutoPlayVideos = false,
            AutoPlayAudio = false,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _dataStore.UserMediaPreferences.Add(defaultPreferences);
        return Task.FromResult(defaultPreferences);
    }

    public Task SaveAsync(UserMediaPreferences preferences, CancellationToken cancellationToken = default)
    {
        var existing = _dataStore.UserMediaPreferences.FirstOrDefault(p => p.UserId == preferences.UserId);
        if (existing is not null)
        {
            _dataStore.UserMediaPreferences.Remove(existing);
        }

        _dataStore.UserMediaPreferences.Add(preferences);
        return Task.CompletedTask;
    }
}
