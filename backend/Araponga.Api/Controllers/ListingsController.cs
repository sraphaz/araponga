using Araponga.Api.Contracts.Common;
using Araponga.Api.Contracts.Marketplace;
using Araponga.Api.Security;
using Araponga.Application.Common;
using Araponga.Application.Services;
using Araponga.Domain.Marketplace;
using Microsoft.AspNetCore.Mvc;

namespace Araponga.Api.Controllers;

[ApiController]
[Route("api/v1/listings")]
[Produces("application/json")]
[Tags("Listings")]
public sealed class ListingsController : ControllerBase
{
    private readonly StoreItemService _itemService;
    private readonly CurrentUserAccessor _currentUserAccessor;

    public ListingsController(StoreItemService itemService, CurrentUserAccessor currentUserAccessor)
    {
        _itemService = itemService;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <summary>
    /// Cria um item (produto ou serviço).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ListingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ListingResponse>> CreateListing(
        [FromBody] CreateListingRequest request,
        CancellationToken cancellationToken)
    {
        if (request.TerritoryId == Guid.Empty || request.StoreId == Guid.Empty)
        {
            return BadRequest(new { error = "territoryId and storeId are required." });
        }

        if (!TryParseItemType(request.Type, out var type))
        {
            return BadRequest(new { error = "Invalid type." });
        }

        if (!TryParseItemPricingType(request.PricingType, out var pricingType))
        {
            return BadRequest(new { error = "Invalid pricingType." });
        }

        var status = ItemStatus.Active;
        if (!string.IsNullOrWhiteSpace(request.Status) && !TryParseItemStatus(request.Status, out status))
        {
            return BadRequest(new { error = "Invalid status." });
        }

        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        var result = await _itemService.CreateItemAsync(
            request.TerritoryId,
            userContext.User.Id,
            request.StoreId,
            type,
            request.Title,
            request.Description,
            request.Category,
            request.Tags,
            pricingType,
            request.PriceAmount,
            request.Currency,
            request.Unit,
            request.Latitude,
            request.Longitude,
            status,
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
        {
            if (result.Error?.Contains("not authorized", StringComparison.OrdinalIgnoreCase) == true)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            return BadRequest(new { error = result.Error ?? "Unable to create item." });
        }

        return CreatedAtAction(nameof(GetListingById), new { id = result.Value.Id }, ToResponse(result.Value));
    }

    /// <summary>
    /// Atualiza um item.
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ListingResponse>> UpdateListing(
        [FromRoute] Guid id,
        [FromBody] UpdateListingRequest request,
        CancellationToken cancellationToken)
    {
        ItemType? type = null;
        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            if (!TryParseItemType(request.Type, out var parsedType))
            {
                return BadRequest(new { error = "Invalid type." });
            }

            type = parsedType;
        }

        ItemPricingType? pricingType = null;
        if (!string.IsNullOrWhiteSpace(request.PricingType))
        {
            if (!TryParseItemPricingType(request.PricingType, out var parsedPricing))
            {
                return BadRequest(new { error = "Invalid pricingType." });
            }

            pricingType = parsedPricing;
        }

        ItemStatus? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!TryParseItemStatus(request.Status, out var parsedStatus))
            {
                return BadRequest(new { error = "Invalid status." });
            }

            status = parsedStatus;
        }

        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        var result = await _itemService.UpdateItemAsync(
            id,
            userContext.User.Id,
            type,
            request.Title,
            request.Description,
            request.Category,
            request.Tags,
            pricingType,
            request.PriceAmount,
            request.Currency,
            request.Unit,
            request.Latitude,
            request.Longitude,
            status,
            cancellationToken);

        if (!result.IsSuccess || result.Value is null)
        {
            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            {
                return NotFound();
            }

            return result.Error?.Contains("not authorized", StringComparison.OrdinalIgnoreCase) == true
                ? StatusCode(StatusCodes.Status403Forbidden)
                : BadRequest(new { error = result.Error ?? "Unable to update listing." });
        }

        return Ok(ToResponse(result.Value));
    }

    /// <summary>
    /// Arquiva um item.
    /// </summary>
    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(typeof(ListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ListingResponse>> ArchiveListing(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userContext = await _currentUserAccessor.GetAsync(Request, cancellationToken);
        if (userContext.Status != TokenStatus.Valid || userContext.User is null)
        {
            return Unauthorized();
        }

        var result = await _itemService.ArchiveItemAsync(id, userContext.User.Id, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
        {
            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            {
                return NotFound();
            }

            return result.Error?.Contains("not authorized", StringComparison.OrdinalIgnoreCase) == true
                ? StatusCode(StatusCodes.Status403Forbidden)
                : BadRequest(new { error = result.Error ?? "Unable to archive item." });
        }

        return Ok(ToResponse(result.Value));
    }

    /// <summary>
    /// Busca items (produtos e serviços) no território.
    /// </summary>
    /// <remarks>
    /// Endpoint público. Visitantes e moradores podem consultar produtos e serviços disponíveis no território.
    /// Por padrão, retorna apenas items ativos (Status=Active).
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ListingResponse>>> SearchListings(
        [FromQuery] Guid territoryId,
        [FromQuery] string? type,
        [FromQuery] string? q,
        [FromQuery] string? category,
        [FromQuery] string? tags,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        if (territoryId == Guid.Empty)
        {
            return BadRequest(new { error = "territoryId is required." });
        }

        ItemType? parsedType = null;
        if (!string.IsNullOrWhiteSpace(type))
        {
            if (!TryParseItemType(type, out var resolvedType))
            {
                return BadRequest(new { error = "Invalid type." });
            }

            parsedType = resolvedType;
        }

        ItemStatus? parsedStatus = ItemStatus.Active;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!TryParseItemStatus(status, out var resolvedStatus))
            {
                return BadRequest(new { error = "Invalid status." });
            }

            parsedStatus = resolvedStatus;
        }

        var items = await _itemService.SearchItemsAsync(
            territoryId,
            parsedType,
            q,
            category,
            tags,
            parsedStatus,
            cancellationToken);

        var response = items.Select(ToResponse).ToList();
        return Ok(response);
    }

    /// <summary>
    /// Busca items (produtos e serviços) no território (paginado).
    /// </summary>
    /// <remarks>
    /// Endpoint público. Visitantes e moradores podem consultar produtos e serviços disponíveis no território.
    /// Por padrão, retorna apenas items ativos (Status=Active).
    /// </remarks>
    [HttpGet("paged")]
    [ProducesResponseType(typeof(PagedResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<ListingResponse>>> SearchListingsPaged(
        [FromQuery] Guid territoryId,
        [FromQuery] string? type,
        [FromQuery] string? q,
        [FromQuery] string? category,
        [FromQuery] string? tags,
        [FromQuery] string? status,
        CancellationToken cancellationToken,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        if (territoryId == Guid.Empty)
        {
            return BadRequest(new { error = "territoryId is required." });
        }

        ItemType? parsedType = null;
        if (!string.IsNullOrWhiteSpace(type))
        {
            if (!TryParseItemType(type, out var resolvedType))
            {
                return BadRequest(new { error = "Invalid type." });
            }

            parsedType = resolvedType;
        }

        ItemStatus? parsedStatus = ItemStatus.Active;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!TryParseItemStatus(status, out var resolvedStatus))
            {
                return BadRequest(new { error = "Invalid status." });
            }

            parsedStatus = resolvedStatus;
        }

        var pagination = new PaginationParameters(pageNumber, pageSize);
        var pagedResult = await _itemService.SearchItemsPagedAsync(
            territoryId,
            parsedType,
            q,
            category,
            tags,
            parsedStatus,
            pagination,
            cancellationToken);

        var response = new PagedResponse<ListingResponse>(
            pagedResult.Items.Select(ToResponse).ToList(),
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalCount,
            pagedResult.TotalPages,
            pagedResult.HasPreviousPage,
            pagedResult.HasNextPage);

        return Ok(response);
    }

    /// <summary>
    /// Obtém detalhes de um item (produto ou serviço).
    /// </summary>
    /// <remarks>
    /// Endpoint público. Visitantes e moradores podem consultar detalhes de produtos e serviços.
    /// </remarks>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ListingResponse>> GetListingById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var item = await _itemService.GetByIdAsync(id, cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(item));
    }

    private static ListingResponse ToResponse(StoreItem listing)
    {
        return new ListingResponse(
            listing.Id,
            listing.TerritoryId,
            listing.StoreId,
            listing.Type.ToString().ToUpperInvariant(),
            listing.Title,
            listing.Description,
            listing.Category,
            listing.Tags,
            listing.PricingType.ToString().ToUpperInvariant(),
            listing.PriceAmount,
            listing.Currency,
            listing.Unit,
            listing.Latitude,
            listing.Longitude,
            listing.Status.ToString().ToUpperInvariant(),
            listing.CreatedAtUtc,
            listing.UpdatedAtUtc);
    }

    private static bool TryParseItemType(string? raw, out ItemType type)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            type = default;
            return false;
        }

        var normalized = raw.Replace("_", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);

        return Enum.TryParse(normalized, true, out type);
    }

    private static bool TryParseItemPricingType(string? raw, out ItemPricingType pricingType)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            pricingType = default;
            return false;
        }

        var normalized = raw.Replace("_", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);

        return Enum.TryParse(normalized, true, out pricingType);
    }

    private static bool TryParseItemStatus(string? raw, out ItemStatus status)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            status = default;
            return false;
        }

        var normalized = raw.Replace("_", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase);

        return Enum.TryParse(normalized, true, out status);
    }
}
