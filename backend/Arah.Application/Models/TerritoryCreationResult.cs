using Arah.Domain.Territories;

namespace Arah.Application.Models;

public sealed record TerritoryCreationResult(bool Success, string? Error, Territory? Territory);
