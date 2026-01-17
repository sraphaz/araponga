using Araponga.Domain.Media;

namespace Araponga.Api.Contracts.Media;

/// <summary>
/// Request para atualizar configuração de mídia de um território.
/// </summary>
public sealed record UpdateTerritoryMediaConfigRequest(
    MediaContentConfigRequest? Posts,
    MediaContentConfigRequest? Events,
    MediaContentConfigRequest? Marketplace,
    MediaChatConfigRequest? Chat);

/// <summary>
/// Configuração de mídia para um tipo de conteúdo.
/// </summary>
public sealed record MediaContentConfigRequest(
    bool? ImagesEnabled,
    bool? VideosEnabled,
    bool? AudioEnabled,
    int? MaxMediaCount,
    int? MaxVideoCount,
    int? MaxAudioCount,
    long? MaxImageSizeBytes,
    long? MaxVideoSizeBytes,
    long? MaxAudioSizeBytes,
    int? MaxVideoDurationSeconds,
    int? MaxAudioDurationSeconds);

/// <summary>
/// Configuração de mídia para Chat.
/// </summary>
public sealed record MediaChatConfigRequest(
    bool? ImagesEnabled,
    bool? AudioEnabled,
    bool? VideosEnabled,
    long? MaxImageSizeBytes,
    long? MaxAudioSizeBytes,
    int? MaxAudioDurationSeconds);
