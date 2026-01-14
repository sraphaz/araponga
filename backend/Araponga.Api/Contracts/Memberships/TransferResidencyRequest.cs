namespace Araponga.Api.Contracts.Memberships;

public sealed record TransferResidencyRequest(
    Guid ToTerritoryId
);
