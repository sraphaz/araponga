namespace Araponga.Domain.Marketplace;

/// <summary>
/// Representa um produto ou serviço oferecido por um morador através de uma Store.
/// StoreItem não é um TerritoryAsset e não pode vender ou transferir TerritoryAssets.
/// Items podem referenciar TerritoryAssets contextualmente (ex.: "Serviço de guia na trilha X"),
/// mas isso não implica propriedade ou venda do asset.
/// </summary>
public sealed class StoreItem
{
    public StoreItem(
        Guid id,
        Guid territoryId,
        Guid storeId,
        ItemType type,
        string title,
        string? description,
        string? category,
        string? tags,
        ItemPricingType pricingType,
        decimal? priceAmount,
        string? currency,
        string? unit,
        double? latitude,
        double? longitude,
        ItemStatus status,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        if (territoryId == Guid.Empty)
        {
            throw new ArgumentException("Territory ID is required.", nameof(territoryId));
        }

        if (storeId == Guid.Empty)
        {
            throw new ArgumentException("Store ID is required.", nameof(storeId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        Id = id;
        TerritoryId = territoryId;
        StoreId = storeId;
        Type = type;
        Title = title.Trim();
        Description = description;
        Category = category;
        Tags = tags;
        PricingType = pricingType;
        PriceAmount = priceAmount;
        Currency = currency;
        Unit = unit;
        Latitude = latitude;
        Longitude = longitude;
        Status = status;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public Guid Id { get; }
    public Guid TerritoryId { get; }
    public Guid StoreId { get; }
    public ItemType Type { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public string? Category { get; private set; }
    public string? Tags { get; private set; }
    public ItemPricingType PricingType { get; private set; }
    public decimal? PriceAmount { get; private set; }
    public string? Currency { get; private set; }
    public string? Unit { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public ItemStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }

    public void UpdateDetails(
        ItemType type,
        string title,
        string? description,
        string? category,
        string? tags,
        ItemPricingType pricingType,
        decimal? priceAmount,
        string? currency,
        string? unit,
        double? latitude,
        double? longitude,
        ItemStatus status,
        DateTime updatedAtUtc)
    {
        Type = type;
        Title = title.Trim();
        Description = description;
        Category = category;
        Tags = tags;
        PricingType = pricingType;
        PriceAmount = priceAmount;
        Currency = currency;
        Unit = unit;
        Latitude = latitude;
        Longitude = longitude;
        Status = status;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Archive(DateTime updatedAtUtc)
    {
        Status = ItemStatus.Archived;
        UpdatedAtUtc = updatedAtUtc;
    }
}
