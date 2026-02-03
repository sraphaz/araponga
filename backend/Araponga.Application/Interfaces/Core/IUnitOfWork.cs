namespace Araponga.Application.Interfaces;

/// <summary>
/// Unidade de trabalho para persistência. Em Postgres a implementação registrada é ArapongaDbContext:
/// CommitAsync persiste as alterações dos repositórios da Infrastructure principal (Territory, User, JoinRequest, etc.).
/// Repositórios dos módulos (Feed, Events, Map, etc.) usam seus próprios DbContexts e não são cobertos por esta interface.
/// Ver docs/VALIDACAO_INFRAESTRUTURA_INTEGRIDADE.md §3.1.
/// </summary>
public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Inicia uma transação explícita.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Reverte uma transação ativa.
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Verifica se há uma transação ativa.
    /// </summary>
    Task<bool> HasActiveTransactionAsync(CancellationToken cancellationToken);
}
