using Arah.Modules.Marketplace.Domain;

namespace Arah.Application.Models;

public sealed record CartItemDetails(
    CartItem Item,
    StoreItem StoreItem,
    Store Store,
    bool IsPurchasable);

public sealed record CartDetails(
    Cart Cart,
    IReadOnlyList<CartItemDetails> Items);
