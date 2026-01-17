using Araponga.Application.Interfaces.Media;
using Araponga.Domain.Media;

namespace Araponga.Infrastructure.InMemory;

public sealed class InMemoryMediaStorageConfigRepository : IMediaStorageConfigRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryMediaStorageConfigRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<MediaStorageConfig?> GetActiveAsync(CancellationToken cancellationToken)
    {
        var config = _dataStore.MediaStorageConfigs.FirstOrDefault(c => c.IsActive);
        return Task.FromResult(config);
    }

    public Task<MediaStorageConfig?> GetByIdAsync(Guid configId, CancellationToken cancellationToken)
    {
        var config = _dataStore.MediaStorageConfigs.FirstOrDefault(c => c.Id == configId);
        return Task.FromResult(config);
    }

    public Task<IReadOnlyList<MediaStorageConfig>> ListAllAsync(CancellationToken cancellationToken)
    {
        var configs = _dataStore.MediaStorageConfigs.ToList();
        return Task.FromResult<IReadOnlyList<MediaStorageConfig>>(configs);
    }

    public Task AddAsync(MediaStorageConfig config, CancellationToken cancellationToken)
    {
        _dataStore.MediaStorageConfigs.Add(config);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(MediaStorageConfig config, CancellationToken cancellationToken)
    {
        // In-memory: a referência já está na lista, então não precisa fazer nada
        // Mas vamos garantir que existe
        var existing = _dataStore.MediaStorageConfigs.FirstOrDefault(c => c.Id == config.Id);
        if (existing is null)
        {
            _dataStore.MediaStorageConfigs.Add(config);
        }
        return Task.CompletedTask;
    }

    public Task DeactivateAllAsync(CancellationToken cancellationToken)
    {
        // Este método não é mais necessário, pois o serviço desativa configurações individualmente
        // Mantido para compatibilidade com a interface, mas não faz nada
        // O serviço usa ListAllAsync e chama Deactivate em cada configuração ativa
        return Task.CompletedTask;
    }
}
