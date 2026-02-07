namespace Arah.Api.Contracts.Memberships;

public sealed record VerifyResidencyGeoRequest(
    double Lat,
    double Lng
);
