namespace Araponga.Application.Models;

public sealed record TerritoryAssetValidationResult(
    TerritoryAssetDetails Asset,
    bool Created);
