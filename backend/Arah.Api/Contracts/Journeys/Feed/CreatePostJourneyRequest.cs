namespace Arah.Api.Contracts.Journeys.Feed;

/// <summary>
/// Request para jornada de criação de post. Em multipart: title, content, type, visibility, territoryId, tags (JSON array), mediaFiles (arquivos).
/// </summary>
public sealed record CreatePostJourneyRequest(
    string Title,
    string Content,
    string Type,
    string Visibility,
    Guid TerritoryId,
    IReadOnlyList<string>? Tags,
    Guid? MapEntityId,
    IReadOnlyList<Guid>? MediaIds = null);
