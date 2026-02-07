using Arah.Api.Contracts.Journeys.Events;
using Arah.Api.Security;
using Arah.Api.Services.Journeys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Arah.Api.Controllers.Journeys;

/// <summary>
/// Jornadas de eventos (BFF v2): lista, criar, participar.
/// </summary>
[ApiController]
[Route("api/v2/journeys/events")]
[Produces("application/json")]
[Tags("BFF - Events")]
public sealed class EventsJourneyController : ControllerBase
{
    private readonly IEventJourneyService _eventJourneyService;
    private readonly CurrentUserAccessor _currentUserAccessor;

    public EventsJourneyController(
        IEventJourneyService eventJourneyService,
        CurrentUserAccessor currentUserAccessor)
    {
        _eventJourneyService = eventJourneyService;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <summary>
    /// Lista eventos do território formatados para UI (detalhes, participantes, mídias, participação do usuário).
    /// </summary>
    [HttpGet("territory-events")]
    [EnableRateLimiting("read")]
    [ProducesResponseType(typeof(TerritoryEventsJourneyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TerritoryEventsJourneyResponse>> GetTerritoryEvents(
        [FromQuery] Guid territoryId,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] string? status = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid)
        {
            return Unauthorized();
        }

        if (territoryId == Guid.Empty)
        {
            return BadRequest(new { error = "territoryId is required." });
        }

        pageSize = Math.Clamp(pageSize, 1, 100);
        var response = await _eventJourneyService.GetTerritoryEventsAsync(
            territoryId,
            userContext.User?.Id,
            from,
            to,
            status,
            pageNumber,
            pageSize,
            cancellationToken);

        if (response is null)
        {
            return BadRequest(new { error = "Could not load events." });
        }

        return Ok(response);
    }

    /// <summary>
    /// Cria um evento no território.
    /// </summary>
    [HttpPost("create-event")]
    [EnableRateLimiting("write")]
    [ProducesResponseType(typeof(CreateEventJourneyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<CreateEventJourneyResponse>> CreateEvent(
        [FromBody] CreateEventJourneyRequest request,
        CancellationToken cancellationToken = default)
    {
        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        var response = await _eventJourneyService.CreateEventAsync(
            request,
            userContext.User.Id,
            cancellationToken);

        if (response is null)
        {
            return BadRequest(new { error = "Invalid request or unable to create event." });
        }

        return CreatedAtAction(nameof(GetTerritoryEvents), new { territoryId = request.TerritoryId }, response);
    }

    /// <summary>
    /// Marca interesse ou confirma participação em um evento.
    /// </summary>
    [HttpPost("participate")]
    [EnableRateLimiting("write")]
    [ProducesResponseType(typeof(EventParticipationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<EventParticipationResponse>> Participate(
        [FromBody] EventParticipationRequest request,
        CancellationToken cancellationToken = default)
    {
        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        var response = await _eventJourneyService.ParticipateAsync(
            request.EventId,
            userContext.User.Id,
            request.Status,
            cancellationToken);

        if (response is null)
        {
            return BadRequest(new { error = "Invalid request or event not found." });
        }

        return Ok(response);
    }
}
