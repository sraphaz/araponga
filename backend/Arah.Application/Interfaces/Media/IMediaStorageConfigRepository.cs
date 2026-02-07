using Arah.Domain.Media;

namespace Arah.Application.Interfaces.Media;

/// <summary>
/// Repositório para gerenciar configurações de blob storage para mídias.
/// </summary>
public interface IMediaStorageConfigRepository
{
    /// <summary>
    /// Obtém a configuração ativa de storage.
    /// </summary>
    Task<MediaStorageConfig?> GetActiveAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém configuração por ID.
    /// </summary>
    Task<MediaStorageConfig?> GetByIdAsync(Guid configId, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todas as configurações de storage.
    /// </summary>
    Task<IReadOnlyList<MediaStorageConfig>> ListAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona nova configuração de storage.
    /// </summary>
    Task AddAsync(MediaStorageConfig config, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza configuração de storage existente.
    /// </summary>
    Task UpdateAsync(MediaStorageConfig config, CancellationToken cancellationToken);

    /// <summary>
    /// Desativa todas as configurações de storage (antes de ativar uma nova).
    /// </summary>
    Task DeactivateAllAsync(CancellationToken cancellationToken);
}
