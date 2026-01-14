namespace Araponga.Domain.Marketplace;

public sealed class CheckoutItem
{
    public CheckoutItem(
        Guid id,
        Guid checkoutId,
        Guid itemId,
        ItemType itemType,
        string titleSnapshot,
        int quantity,
        decimal? unitPrice,
        decimal? lineSubtotal,
        decimal? platformFeeLine,
        decimal? lineTotal,
        DateTime createdAtUtc)
    {
        if (checkoutId == Guid.Empty)
        {
            throw new ArgumentException("Checkout ID is required.", nameof(checkoutId));
        }

        if (itemId == Guid.Empty)
        {
            throw new ArgumentException("Item ID is required.", nameof(itemId));
        }

        if (string.IsNullOrWhiteSpace(titleSnapshot))
        {
            throw new ArgumentException("Title snapshot is required.", nameof(titleSnapshot));
        }

        if (quantity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");
        }

        Id = id;
        CheckoutId = checkoutId;
        ItemId = itemId;
        ItemType = itemType;
        TitleSnapshot = titleSnapshot.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineSubtotal = lineSubtotal;
        PlatformFeeLine = platformFeeLine;
        LineTotal = lineTotal;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }
    public Guid CheckoutId { get; }
    public Guid ItemId { get; }
    public ItemType ItemType { get; }
    public string TitleSnapshot { get; }
    public int Quantity { get; }
    public decimal? UnitPrice { get; }
    public decimal? LineSubtotal { get; }
    public decimal? PlatformFeeLine { get; }
    public decimal? LineTotal { get; }
    public DateTime CreatedAtUtc { get; }
}
