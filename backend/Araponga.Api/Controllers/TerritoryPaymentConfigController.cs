using Araponga.Api.Contracts.Payments;
using Araponga.Api.Security;
using Araponga.Application.Services;
using Araponga.Domain.Marketplace;
using Araponga.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Araponga.Api.Controllers;

[ApiController]
[Route("api/v1/territories/{territoryId:guid}/payment-config")]
[Produces("application/json")]
[Tags("Payment Configuration")]
public sealed class TerritoryPaymentConfigController : ControllerBase
{
    private readonly TerritoryPaymentConfigService _configService;
    private readonly CurrentUserAccessor _currentUserAccessor;
    private readonly AccessEvaluator _accessEvaluator;

    public TerritoryPaymentConfigController(
        TerritoryPaymentConfigService configService,
        CurrentUserAccessor currentUserAccessor,
        AccessEvaluator accessEvaluator)
    {
        _configService = configService;
        _currentUserAccessor = currentUserAccessor;
        _accessEvaluator = accessEvaluator;
    }

    /// <summary>
    /// Obtém a configuração ativa de pagamento do território.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(TerritoryPaymentConfigResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TerritoryPaymentConfigResponse>> GetConfig(
        [FromRoute] Guid territoryId,
        CancellationToken cancellationToken)
    {
        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        var result = await _configService.GetActiveConfigAsync(territoryId, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error });
        }

        var config = result.Value!;
        return Ok(new TerritoryPaymentConfigResponse(
            config.Id,
            config.TerritoryId,
            config.GatewayProvider,
            config.IsActive,
            config.Currency,
            config.MinimumAmount,
            config.MaximumAmount,
            config.ShowFeeBreakdown,
            config.FeeTransparencyLevel.ToString()));
    }

    /// <summary>
    /// Cria ou atualiza a configuração de pagamento do território.
    /// Requer permissão de Curator ou SystemAdmin.
    /// </summary>
    [HttpPut]
    [EnableRateLimiting("write")]
    [ProducesResponseType(typeof(TerritoryPaymentConfigResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<TerritoryPaymentConfigResponse>> UpsertConfig(
        [FromRoute] Guid territoryId,
        [FromBody] UpsertPaymentConfigRequest request,
        CancellationToken cancellationToken)
    {
        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        // Verificar permissão (Curator ou SystemAdmin)
        var isSystemAdmin = await _accessEvaluator.HasSystemPermissionAsync(
            userContext.User.Id,
            SystemPermissionType.SystemAdmin,
            cancellationToken);

        var isCurator = !isSystemAdmin && await _accessEvaluator.HasCapabilityAsync(
            userContext.User.Id,
            territoryId,
            Araponga.Domain.Membership.MembershipCapabilityType.Curator,
            cancellationToken);

        if (!isSystemAdmin && !isCurator)
        {
            return Forbid();
        }

        if (!Enum.TryParse<FeeTransparencyLevel>(request.FeeTransparencyLevel, ignoreCase: true, out var transparencyLevel))
        {
            return BadRequest(new { error = "Invalid feeTransparencyLevel." });
        }

        var result = await _configService.UpsertConfigAsync(
            territoryId,
            userContext.User.Id,
            request.GatewayProvider,
            request.IsActive,
            request.Currency,
            request.MinimumAmount,
            request.MaximumAmount,
            request.ShowFeeBreakdown,
            transparencyLevel,
            cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error ?? "Unable to update payment configuration." });
        }

        var config = result.Value!;
        return Ok(new TerritoryPaymentConfigResponse(
            config.Id,
            config.TerritoryId,
            config.GatewayProvider,
            config.IsActive,
            config.Currency,
            config.MinimumAmount,
            config.MaximumAmount,
            config.ShowFeeBreakdown,
            config.FeeTransparencyLevel.ToString()));
    }

    /// <summary>
    /// Calcula o breakdown de fees para transparência.
    /// </summary>
    [HttpPost("calculate-fees")]
    [EnableRateLimiting("read")]
    [ProducesResponseType(typeof(FeeBreakdownResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<FeeBreakdownResponse>> CalculateFees(
        [FromRoute] Guid territoryId,
        [FromBody] CalculateFeesRequest request,
        CancellationToken cancellationToken)
    {
        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        if (!Enum.TryParse<ItemType>(request.ItemType, ignoreCase: true, out var itemType))
        {
            return BadRequest(new { error = "Invalid itemType." });
        }

        var result = await _configService.CalculateFeeBreakdownAsync(
            territoryId,
            request.AmountInCents,
            itemType,
            cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error ?? "Unable to calculate fees." });
        }

        var breakdown = result.Value!;
        return Ok(new FeeBreakdownResponse(
            breakdown.SubtotalInCents,
            breakdown.PlatformFeeInCents,
            breakdown.TotalInCents,
            breakdown.Currency,
            breakdown.PlatformFeePercent,
            breakdown.PlatformFeeFixed,
            breakdown.ShowBreakdown,
            breakdown.TransparencyLevel.ToString()));
    }
}
