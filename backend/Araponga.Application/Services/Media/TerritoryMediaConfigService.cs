using Araponga.Application.Interfaces;
using Araponga.Application.Interfaces.Media;
using Araponga.Application.Models;
using Araponga.Domain.Media;

namespace Araponga.Application.Services.Media;

/// <summary>
/// Service para gerenciar configurações de mídia por território.
/// </summary>
public sealed class TerritoryMediaConfigService
{
    private readonly ITerritoryMediaConfigRepository _configRepository;
    private readonly IFeatureFlagService _featureFlagService;
    private readonly IUnitOfWork _unitOfWork;

    public TerritoryMediaConfigService(
        ITerritoryMediaConfigRepository configRepository,
        IFeatureFlagService featureFlagService,
        IUnitOfWork unitOfWork)
    {
        _configRepository = configRepository;
        _featureFlagService = featureFlagService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtém configuração de mídia para um território (com valores padrão se não existir).
    /// </summary>
    public async Task<TerritoryMediaConfig> GetConfigAsync(Guid territoryId, CancellationToken cancellationToken = default)
    {
        return await _configRepository.GetOrCreateDefaultAsync(territoryId, cancellationToken);
    }

    /// <summary>
    /// Atualiza configuração de mídia para um território.
    /// </summary>
    public async Task<TerritoryMediaConfig> UpdateConfigAsync(
        Guid territoryId,
        TerritoryMediaConfig config,
        Guid updatedByUserId,
        CancellationToken cancellationToken = default)
    {
        if (config.TerritoryId != territoryId)
        {
            throw new ArgumentException("Territory ID mismatch.", nameof(config));
        }

        config.UpdatedAtUtc = DateTime.UtcNow;
        config.UpdatedByUserId = updatedByUserId;

        await _configRepository.SaveAsync(config, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return config;
    }

    /// <summary>
    /// Verifica se um tipo de mídia está habilitado para um território e tipo de conteúdo.
    /// </summary>
    public async Task<bool> IsMediaTypeEnabledAsync(
        Guid territoryId,
        MediaType mediaType,
        MediaContentType contentType,
        CancellationToken cancellationToken = default)
    {
        // Verificar feature flags primeiro
        var featureFlagEnabled = mediaType switch
        {
            MediaType.Image => _featureFlagService.IsEnabled(territoryId, FeatureFlag.MediaImagesEnabled),
            MediaType.Video => _featureFlagService.IsEnabled(territoryId, FeatureFlag.MediaVideosEnabled),
            MediaType.Audio => _featureFlagService.IsEnabled(territoryId, FeatureFlag.MediaAudioEnabled),
            _ => false
        };

        if (!featureFlagEnabled && contentType != MediaContentType.Chat)
        {
            return false;
        }

        // Verificar configuração específica do território
        var config = await GetConfigAsync(territoryId, cancellationToken);

        return contentType switch
        {
            MediaContentType.Posts => IsMediaTypeEnabledInConfig(config.Posts, mediaType),
            MediaContentType.Events => IsMediaTypeEnabledInConfig(config.Events, mediaType),
            MediaContentType.Marketplace => IsMediaTypeEnabledInConfig(config.Marketplace, mediaType),
            MediaContentType.Chat => IsMediaTypeEnabledInChat(config.Chat, mediaType),
            _ => false
        };
    }

    private static bool IsMediaTypeEnabledInConfig(MediaContentConfig config, MediaType mediaType)
    {
        return mediaType switch
        {
            MediaType.Image => config.ImagesEnabled,
            MediaType.Video => config.VideosEnabled,
            MediaType.Audio => config.AudioEnabled,
            _ => false
        };
    }

    private static bool IsMediaTypeEnabledInChat(MediaChatConfig config, MediaType mediaType)
    {
        // Verificar feature flags específicas do chat
        // TODO: Injetar IFeatureFlagService para verificar ChatMediaImagesEnabled e ChatMediaAudioEnabled
        // Por enquanto, usar apenas configuração

        return mediaType switch
        {
            MediaType.Image => config.ImagesEnabled,
            MediaType.Audio => config.AudioEnabled,
            MediaType.Video => config.VideosEnabled, // Sempre false para chat
            _ => false
        };
    }

    /// <summary>
    /// Obtém limites configurados para um tipo de conteúdo.
    /// </summary>
    public async Task<MediaContentConfig> GetContentConfigAsync(
        Guid territoryId,
        MediaContentType contentType,
        CancellationToken cancellationToken = default)
    {
        var config = await GetConfigAsync(territoryId, cancellationToken);

        return contentType switch
        {
            MediaContentType.Posts => config.Posts,
            MediaContentType.Events => config.Events,
            MediaContentType.Marketplace => config.Marketplace,
            _ => throw new ArgumentException($"Invalid content type: {contentType}", nameof(contentType))
        };
    }

    /// <summary>
    /// Obtém configuração de chat.
    /// </summary>
    public async Task<MediaChatConfig> GetChatConfigAsync(
        Guid territoryId,
        CancellationToken cancellationToken = default)
    {
        var config = await GetConfigAsync(territoryId, cancellationToken);
        return config.Chat;
    }
}

/// <summary>
/// Tipo de conteúdo que pode ter mídias associadas.
/// </summary>
public enum MediaContentType
{
    Posts = 1,
    Events = 2,
    Marketplace = 3,
    Chat = 4
}
