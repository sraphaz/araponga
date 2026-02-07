namespace Arah.Api.Contracts.Users;

/// <summary>
/// Request para atualizar preferências de mídia do usuário.
/// </summary>
public sealed record UpdateUserMediaPreferencesRequest(
    bool? ShowImages,
    bool? ShowVideos,
    bool? ShowAudio,
    bool? AutoPlayVideos,
    bool? AutoPlayAudio);
