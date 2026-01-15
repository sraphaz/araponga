using Araponga.Domain.Marketplace;

namespace Araponga.Application.Interfaces;

/// <summary>
/// Repositório para configurações de pagamento por território.
/// </summary>
public interface ITerritoryPaymentConfigRepository
{
    /// <summary>
    /// Obtém a configuração ativa de pagamento para um território.
    /// </summary>
    Task<TerritoryPaymentConfig?> GetActiveAsync(Guid territoryId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém uma configuração por ID.
    /// </summary>
    Task<TerritoryPaymentConfig?> GetByIdAsync(Guid configId, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todas as configurações de um território (ativas e inativas).
    /// </summary>
    Task<IReadOnlyList<TerritoryPaymentConfig>> ListByTerritoryAsync(Guid territoryId, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona uma nova configuração.
    /// </summary>
    Task AddAsync(TerritoryPaymentConfig config, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza uma configuração existente.
    /// </summary>
    Task UpdateAsync(TerritoryPaymentConfig config, CancellationToken cancellationToken);
}
