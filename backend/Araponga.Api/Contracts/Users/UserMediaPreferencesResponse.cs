using Araponga.Domain.Users;

namespace Araponga.Api.Contracts.Users;

/// <summary>
/// Resposta das preferências de mídia do usuário.
/// </summary>
public sealed record UserMediaPreferencesResponse(
    Guid UserId,
    bool ShowImages,
    bool ShowVideos,
    bool ShowAudio,
    bool AutoPlayVideos,
    bool AutoPlayAudio,
    DateTime UpdatedAtUtc);
