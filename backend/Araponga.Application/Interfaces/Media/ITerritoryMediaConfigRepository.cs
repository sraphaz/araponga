using Araponga.Domain.Media;

namespace Araponga.Application.Interfaces.Media;

/// <summary>
/// Repositório para configurações de mídia por território.
/// </summary>
public interface ITerritoryMediaConfigRepository
{
    /// <summary>
    /// Obtém configuração de mídia para um território.
    /// </summary>
    Task<TerritoryMediaConfig?> GetByTerritoryIdAsync(Guid territoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém ou cria configuração padrão para um território.
    /// </summary>
    Task<TerritoryMediaConfig> GetOrCreateDefaultAsync(Guid territoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva ou atualiza configuração de mídia.
    /// </summary>
    Task SaveAsync(TerritoryMediaConfig config, CancellationToken cancellationToken = default);
}
