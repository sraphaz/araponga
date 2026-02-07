namespace Arah.Api.Contracts.Map;

public sealed record SuggestMapEntityRequest(
    string Name,
    string Category,
    double Latitude,
    double Longitude
);
