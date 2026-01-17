using Araponga.Application.Interfaces;
using Araponga.Application.Interfaces.Users;
using Araponga.Domain.Users;

namespace Araponga.Application.Services.Users;

/// <summary>
/// Service para gerenciar preferências de mídia do usuário.
/// </summary>
public sealed class UserMediaPreferencesService
{
    private readonly IUserMediaPreferencesRepository _preferencesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserMediaPreferencesService(
        IUserMediaPreferencesRepository preferencesRepository,
        IUnitOfWork unitOfWork)
    {
        _preferencesRepository = preferencesRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtém preferências de mídia para um usuário (com valores padrão se não existir).
    /// </summary>
    public async Task<UserMediaPreferences> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _preferencesRepository.GetOrCreateDefaultAsync(userId, cancellationToken);
    }

    /// <summary>
    /// Atualiza preferências de mídia para um usuário.
    /// </summary>
    public async Task<UserMediaPreferences> UpdatePreferencesAsync(
        Guid userId,
        UserMediaPreferences preferences,
        CancellationToken cancellationToken = default)
    {
        if (preferences.UserId != userId)
        {
            throw new ArgumentException("User ID mismatch.", nameof(preferences));
        }

        preferences.UpdatedAtUtc = DateTime.UtcNow;

        await _preferencesRepository.SaveAsync(preferences, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return preferences;
    }
}
