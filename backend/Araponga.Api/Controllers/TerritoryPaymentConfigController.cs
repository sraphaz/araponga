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

        // Validar gateway provider (será validado no service também, mas validar aqui também)
        if (string.IsNullOrWhiteSpace(request.GatewayProvider) || request.GatewayProvider.Length > 100)
        {
            return BadRequest(new { error = "Gateway provider is required and must be at most 100 characters." });
        }

        // Validar currency
        if (string.IsNullOrWhiteSpace(request.Currency) || request.Currency.Length > 10)
        {
            return BadRequest(new { error = "Currency is required and must be at most 10 characters." });
        }

        // Validar minimumAmount
        if (request.MinimumAmount < 0)
        {
            return BadRequest(new { error = "Minimum amount must be non-negative." });
        }

        // Validar maximumAmount
        if (request.MaximumAmount.HasValue)
        {
            if (request.MaximumAmount.Value < 0)
            {
                return BadRequest(new { error = "Maximum amount must be non-negative." });
            }
            if (request.MaximumAmount.Value < request.MinimumAmount)
            {
                return BadRequest(new { error = "Maximum amount must be greater than or equal to minimum amount." });
            }
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

        // Validar amountInCents
        if (request.AmountInCents <= 0)
        {
            return BadRequest(new { error = "Amount must be positive." });
        }

        // Validar limites de amount (prevenir valores extremos)
        if (request.AmountInCents > 1_000_000_000) // 10 milhões (em centavos)
        {
            return BadRequest(new { error = "Amount exceeds maximum allowed value." });
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
