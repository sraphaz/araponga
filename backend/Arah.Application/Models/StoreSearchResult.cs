using Arah.Modules.Marketplace.Domain;

namespace Arah.Application.Models;

public sealed record StoreSearchResult(
    Store Store,
    double AverageRating);
