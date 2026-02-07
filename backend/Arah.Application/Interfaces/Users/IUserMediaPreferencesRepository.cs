using Arah.Domain.Users;

namespace Arah.Application.Interfaces.Users;

/// <summary>
/// Repositório para preferências de mídia do usuário.
/// </summary>
public interface IUserMediaPreferencesRepository
{
    /// <summary>
    /// Obtém preferências de mídia para um usuário.
    /// </summary>
    Task<UserMediaPreferences?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém ou cria preferências padrão para um usuário.
    /// </summary>
    Task<UserMediaPreferences> GetOrCreateDefaultAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva ou atualiza preferências de mídia.
    /// </summary>
    Task SaveAsync(UserMediaPreferences preferences, CancellationToken cancellationToken = default);
}
