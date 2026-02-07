using Arah.Application.Common;
using Arah.Application.Interfaces;
using Arah.Application.Interfaces.Media;
using Arah.Application.Models;
using Arah.Application.Services;
using Arah.Domain.Events;
using Arah.Domain.Media;

namespace Arah.Api.Services.Journeys.Backend;

public sealed class InProcessEventsJourneyBackend : IEventsJourneyBackend
{
    private readonly EventsService _eventsService;
    private readonly MediaService _mediaService;
    private readonly IEventParticipationRepository _participationRepository;

    public InProcessEventsJourneyBackend(
        EventsService eventsService,
        MediaService mediaService,
        IEventParticipationRepository participationRepository)
    {
        _eventsService = eventsService;
        _mediaService = mediaService;
        _participationRepository = participationRepository;
    }

    public async Task<BackendPagedResult<BackendEventDetail>> ListEventsPagedAsync(
        Guid territoryId,
        DateTime? from,
        DateTime? to,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        EventStatus? parsedStatus = null;
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<EventStatus>(status, true, out var ps))
            parsedStatus = ps;
        var pagination = new PaginationParameters(pageNumber, pageSize);
        var paged = await _eventsService.ListEventsPagedAsync(territoryId, from, to, parsedStatus, pagination, cancellationToken);
        var items = paged.Items.Select(ToBackendEventDetail).ToList();
        return new BackendPagedResult<BackendEventDetail>(
            items, paged.PageNumber, paged.PageSize, paged.TotalCount, paged.TotalPages,
            paged.HasPreviousPage, paged.HasNextPage);
    }

    public async Task<IReadOnlyList<BackendEventParticipationStatus>> GetUserParticipationsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var list = await _participationRepository.GetByUserIdAsync(userId, cancellationToken);
        return list.Select(p => new BackendEventParticipationStatus(p.EventId, p.Status.ToString().ToUpperInvariant())).ToList();
    }

    public async Task<IReadOnlyDictionary<Guid, BackendEventDetail>> GetEventDetailsByIdsAsync(
        IReadOnlyCollection<Guid> eventIds,
        CancellationToken cancellationToken = default)
    {
        var dict = await _eventsService.GetSummariesByIdsAsync(eventIds, cancellationToken);
        return dict.ToDictionary(kv => kv.Key, kv => ToBackendEventDetail(kv.Value));
    }

    public async Task<(string? CoverUrl, IReadOnlyList<string> AdditionalUrls)> GetEventMediaUrlsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        var assets = await _mediaService.ListMediaByOwnerAsync(MediaOwnerType.Event, eventId, cancellationToken);
        if (assets.Count == 0)
            return (null, Array.Empty<string>());
        string? cover = null;
        var additional = new List<string>();
        var isFirst = true;
        foreach (var a in assets)
        {
            var urlResult = await _mediaService.GetMediaUrlAsync(a.Id, null, cancellationToken);
            if (urlResult.IsSuccess && urlResult.Value is not null)
            {
                if (isFirst) { cover = urlResult.Value; isFirst = false; }
                else additional.Add(urlResult.Value);
            }
        }
        return (cover, additional);
    }

    public async Task<BackendCreateEventResult> CreateEventAsync(
        Guid territoryId,
        Guid userId,
        string title,
        string? description,
        DateTime startsAtUtc,
        DateTime? endsAtUtc,
        double? latitude,
        double? longitude,
        string? locationLabel,
        Guid? coverMediaId,
        IReadOnlyList<Guid>? additionalMediaIds,
        CancellationToken cancellationToken = default)
    {
        var result = await _eventsService.CreateEventAsync(
            territoryId, userId, title, description, startsAtUtc, endsAtUtc,
            latitude ?? 0, longitude ?? 0, locationLabel, coverMediaId, additionalMediaIds, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
            return new BackendCreateEventResult(false, null, result.Error);
        return new BackendCreateEventResult(true, ToBackendEventDetail(result.Value), null);
    }

    public async Task<(bool Success, string? Error)> SetParticipationAsync(
        Guid eventId,
        Guid userId,
        string status,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<EventParticipationStatus>(status, true, out var st))
            return (false, "Invalid status.");
        var result = await _eventsService.SetParticipationAsync(eventId, userId, st, cancellationToken);
        return (result.IsSuccess, result.IsSuccess ? null : result.Error);
    }

    private static BackendEventDetail ToBackendEventDetail(EventSummary s)
    {
        var e = s.Event;
        return new BackendEventDetail(
            e.Id, e.TerritoryId, e.Title, e.Description, e.StartsAtUtc, e.EndsAtUtc,
            e.Latitude, e.Longitude, e.LocationLabel, e.Status.ToString().ToUpperInvariant(),
            e.CreatedByMembership.ToString().ToUpperInvariant(), s.CreatedByDisplayName,
            s.InterestedCount, s.ConfirmedCount);
    }
}
