using Araponga.Api.Contracts.Health;
using Araponga.Api.Security;
using Araponga.Application.Services;
using Araponga.Domain.Health;
using Microsoft.AspNetCore.Mvc;

namespace Araponga.Api.Controllers;

[ApiController]
[Route("api/v1/health")]
[Produces("application/json")]
[Tags("Health")]
public sealed class HealthController : ControllerBase
{
    private readonly ActiveTerritoryService _activeTerritoryService;
    private readonly CurrentUserAccessor _currentUserAccessor;
    private readonly AccessEvaluator _accessEvaluator;
    private readonly HealthService _healthService;

    public HealthController(
        ActiveTerritoryService activeTerritoryService,
        CurrentUserAccessor currentUserAccessor,
        AccessEvaluator accessEvaluator,
        HealthService healthService)
    {
        _activeTerritoryService = activeTerritoryService;
        _currentUserAccessor = currentUserAccessor;
        _accessEvaluator = accessEvaluator;
        _healthService = healthService;
    }

    /// <summary>
    /// Visualiza indicadores de saúde do território ativo.
    /// </summary>
    [HttpGet("indicators")]
    [ProducesResponseType(typeof(IEnumerable<HealthIndicatorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<HealthIndicatorResponse>>> GetIndicators(
        [FromQuery] Guid? territoryId,
        CancellationToken cancellationToken)
    {
        var resolvedTerritoryId = await ResolveTerritoryIdAsync(territoryId, cancellationToken);
        if (resolvedTerritoryId is null)
        {
            return BadRequest(new { error = "territoryId (query) or X-Session-Id header is required." });
        }

        var indicators = new List<HealthIndicatorResponse>
        {
            new("Água Potável", "OK", "Monitoramento básico do território."),
            new("Nascentes", "ATENÇÃO", "Relatos recentes aguardando validação comunitária."),
            new("Árvores Nativas", "OK", "Inventário atualizado.")
        };

        return Ok(indicators);
    }

    /// <summary>
    /// Reporta um alerta ambiental.
    /// </summary>
    [HttpPost("alerts")]
    [ProducesResponseType(typeof(HealthAlertResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<HealthAlertResponse>> ReportAlert(
        [FromQuery] Guid? territoryId,
        [FromBody] ReportAlertRequest request,
        CancellationToken cancellationToken)
    {
        var resolvedTerritoryId = await ResolveTerritoryIdAsync(territoryId, cancellationToken);
        if (resolvedTerritoryId is null)
        {
            return BadRequest(new { error = "territoryId (query) or X-Session-Id header is required." });
        }

        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userContext.User.Id, resolvedTerritoryId.Value, cancellationToken);
        if (!isResident)
        {
            return Unauthorized();
        }

        var result = await _healthService.ReportAlertAsync(
            resolvedTerritoryId.Value,
            userContext.User.Id,
            request.Title,
            request.Description,
            cancellationToken);

        if (!result.success || result.alert is null)
        {
            return BadRequest(new { error = result.error ?? "Unable to report alert." });
        }

        var response = new HealthAlertResponse(
            result.alert.Id,
            result.alert.Title,
            result.alert.Description,
            result.alert.Status.ToString().ToUpperInvariant(),
            result.alert.CreatedAtUtc);

        return CreatedAtAction(nameof(ReportAlert), new { }, response);
    }

    /// <summary>
    /// Valida alerta ambiental (curadoria).
    /// </summary>
    [HttpPatch("alerts/{alertId:guid}/validation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ValidateAlert(
        [FromRoute] Guid alertId,
        [FromQuery] Guid? territoryId,
        [FromBody] ValidateAlertRequest request,
        CancellationToken cancellationToken)
    {
        var resolvedTerritoryId = await ResolveTerritoryIdAsync(territoryId, cancellationToken);
        if (resolvedTerritoryId is null)
        {
            return BadRequest(new { error = "territoryId (query) or X-Session-Id header is required." });
        }

        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        if (!_accessEvaluator.IsCurator(userContext.User))
        {
            return Unauthorized();
        }

        if (!Enum.TryParse<HealthAlertStatus>(request.Status, true, out var status))
        {
            return BadRequest(new { error = "Invalid status." });
        }

        var success = await _healthService.ValidateAlertAsync(
            resolvedTerritoryId.Value,
            alertId,
            userContext.User.Id,
            status,
            cancellationToken);

        return success ? NoContent() : BadRequest(new { error = "Alert not found." });
    }

    private string? GetSessionId()
    {
        return Request.Headers.TryGetValue(ApiHeaders.SessionId, out var header) &&
               !string.IsNullOrWhiteSpace(header)
            ? header.ToString()
            : null;
    }

    private async Task<Guid?> ResolveTerritoryIdAsync(Guid? territoryId, CancellationToken cancellationToken)
    {
        if (territoryId is not null)
        {
            return territoryId;
        }

        var sessionId = GetSessionId();
        if (sessionId is null)
        {
            return null;
        }

        return await _activeTerritoryService.GetActiveAsync(sessionId, cancellationToken);
    }
}
