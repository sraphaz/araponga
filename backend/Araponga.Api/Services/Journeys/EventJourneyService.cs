using Araponga.Api.Contracts.Journeys.Common;
using Araponga.Api.Contracts.Journeys.Events;
using Araponga.Api.Services.Journeys.Backend;
using Microsoft.Extensions.Logging;

namespace Araponga.Api.Services.Journeys;

public sealed class EventJourneyService : IEventJourneyService
{
    private readonly IEventsJourneyBackend _backend;
    private readonly ILogger<EventJourneyService> _logger;

    public EventJourneyService(
        IEventsJourneyBackend backend,
        ILogger<EventJourneyService> logger)
    {
        _backend = backend;
        _logger = logger;
    }

    public async Task<TerritoryEventsJourneyResponse?> GetTerritoryEventsAsync(
        Guid territoryId,
        Guid? userId,
        DateTime? from,
        DateTime? to,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var pagedResult = await _backend.ListEventsPagedAsync(
            territoryId, from, to, status, pageNumber, pageSize, cancellationToken);

        Dictionary<Guid, string>? userParticipationByEvent = null;
        if (userId.HasValue)
        {
            var participations = await _backend.GetUserParticipationsAsync(userId.Value, cancellationToken);
            var eventIds = pagedResult.Items.Select(e => e.Id).ToHashSet();
            userParticipationByEvent = participations
                .Where(p => eventIds.Contains(p.EventId))
                .ToDictionary(p => p.EventId, p => p.Status);
        }

        var items = new List<EventItemJourneyDto>();
        foreach (var evt in pagedResult.Items)
        {
            var (coverUrl, additionalUrls) = await _backend.GetEventMediaUrlsAsync(evt.Id, cancellationToken);
            var mediaList = new List<EventMediaDto>();
            if (coverUrl is not null)
                mediaList.Add(new EventMediaDto(coverUrl, "IMAGE", null));
            foreach (var url in additionalUrls)
                mediaList.Add(new EventMediaDto(url, "IMAGE", null));

            var eventDto = MapToEventDetailDto(evt);
            var participantsDto = new EventParticipantsSummaryDto(evt.InterestedCount, evt.ConfirmedCount);
            UserEventParticipationDto? userPart = null;
            if (userId.HasValue && userParticipationByEvent?.TryGetValue(evt.Id, out var partStatus) == true)
                userPart = new UserEventParticipationDto(partStatus);

            items.Add(new EventItemJourneyDto(eventDto, participantsDto, mediaList, userPart));
        }

        const int maxInt32 = int.MaxValue;
        var totalCount = pagedResult.TotalCount > maxInt32 ? maxInt32 : pagedResult.TotalCount;
        var totalPages = pagedResult.TotalPages > maxInt32 ? maxInt32 : pagedResult.TotalPages;
        var paginationDto = new JourneyPaginationDto(
            pagedResult.PageNumber, pagedResult.PageSize, totalCount, totalPages,
            pagedResult.HasPreviousPage, pagedResult.HasNextPage);

        return new TerritoryEventsJourneyResponse(items, paginationDto);
    }

    public async Task<CreateEventJourneyResponse?> CreateEventAsync(
        CreateEventJourneyRequest request,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var result = await _backend.CreateEventAsync(
            request.TerritoryId,
            userId,
            request.Title,
            request.Description,
            request.StartsAtUtc,
            request.EndsAtUtc,
            request.Latitude,
            request.Longitude,
            request.LocationLabel,
            request.CoverMediaId,
            request.AdditionalMediaIds,
            cancellationToken);

        if (!result.Success || result.Event is null)
            return null;

        var (coverUrl, additionalUrls) = await _backend.GetEventMediaUrlsAsync(result.Event.Id, cancellationToken);
        var mediaUrls = new List<string>();
        if (coverUrl is not null) mediaUrls.Add(coverUrl);
        mediaUrls.AddRange(additionalUrls);
        var eventDto = MapToEventDetailDto(result.Event);
        return new CreateEventJourneyResponse(eventDto, mediaUrls);
    }

    public async Task<EventParticipationResponse?> ParticipateAsync(
        Guid eventId,
        Guid userId,
        string status,
        CancellationToken cancellationToken = default)
    {
        var setResult = await _backend.SetParticipationAsync(eventId, userId, status, cancellationToken);
        if (!setResult.Success)
            return null;

        var detailsDict = await _backend.GetEventDetailsByIdsAsync(new[] { eventId }, cancellationToken);
        if (!detailsDict.TryGetValue(eventId, out var evt))
            return null;

        var eventDto = MapToEventDetailDto(evt);
        var participantsDto = new EventParticipantsSummaryDto(evt.InterestedCount, evt.ConfirmedCount);
        var userPart = new UserEventParticipationDto(status.ToUpperInvariant());
        return new EventParticipationResponse(eventDto, participantsDto, userPart);
    }

    private static EventDetailJourneyDto MapToEventDetailDto(BackendEventDetail evt)
    {
        return new EventDetailJourneyDto(
            evt.Id,
            evt.TerritoryId,
            evt.Title,
            evt.Description,
            evt.StartsAtUtc,
            evt.EndsAtUtc ?? evt.StartsAtUtc,
            evt.Latitude,
            evt.Longitude,
            evt.LocationLabel,
            evt.Status,
            evt.CreatedByMembership,
            evt.CreatedByDisplayName);
    }
}
