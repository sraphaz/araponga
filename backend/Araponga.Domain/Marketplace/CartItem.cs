namespace Araponga.Domain.Marketplace;

public sealed class CartItem
{
    public CartItem(
        Guid id,
        Guid cartId,
        Guid itemId,
        int quantity,
        string? notes,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Cart ID is required.", nameof(cartId));
        }

        if (itemId == Guid.Empty)
        {
            throw new ArgumentException("Item ID is required.", nameof(itemId));
        }

        if (quantity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");
        }

        Id = id;
        CartId = cartId;
        ItemId = itemId;
        Quantity = quantity;
        Notes = notes;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public Guid Id { get; }
    public Guid CartId { get; }
    public Guid ItemId { get; }
    public int Quantity { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }

    public void Update(int quantity, string? notes, DateTime updatedAtUtc)
    {
        Quantity = quantity;
        Notes = notes;
        UpdatedAtUtc = updatedAtUtc;
    }
}
