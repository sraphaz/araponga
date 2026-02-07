using Arah.Domain.Media;

namespace Arah.Api.Contracts.Media;

/// <summary>
/// Resposta da configuração de mídia de um território.
/// </summary>
public sealed record TerritoryMediaConfigResponse(
    Guid TerritoryId,
    MediaContentConfigResponse Posts,
    MediaContentConfigResponse Events,
    MediaContentConfigResponse Marketplace,
    MediaChatConfigResponse Chat,
    DateTime UpdatedAtUtc,
    Guid? UpdatedByUserId);

/// <summary>
/// Configuração de mídia para um tipo de conteúdo.
/// </summary>
public sealed record MediaContentConfigResponse(
    bool ImagesEnabled,
    bool VideosEnabled,
    bool AudioEnabled,
    int MaxMediaCount,
    int MaxVideoCount,
    int MaxAudioCount,
    long MaxImageSizeBytes,
    long MaxVideoSizeBytes,
    long MaxAudioSizeBytes,
    int? MaxVideoDurationSeconds,
    int? MaxAudioDurationSeconds,
    IReadOnlyList<string>? AllowedImageMimeTypes,
    IReadOnlyList<string>? AllowedVideoMimeTypes,
    IReadOnlyList<string>? AllowedAudioMimeTypes);

/// <summary>
/// Configuração de mídia para Chat.
/// </summary>
public sealed record MediaChatConfigResponse(
    bool ImagesEnabled,
    bool AudioEnabled,
    bool VideosEnabled,
    long MaxImageSizeBytes,
    long MaxAudioSizeBytes,
    int? MaxAudioDurationSeconds,
    IReadOnlyList<string>? AllowedImageMimeTypes,
    IReadOnlyList<string>? AllowedAudioMimeTypes);
