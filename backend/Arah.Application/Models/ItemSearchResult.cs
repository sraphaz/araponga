using Arah.Modules.Marketplace.Domain;

namespace Arah.Application.Models;

public sealed record ItemSearchResult(
    StoreItem Item,
    double AverageRating);
