namespace Arah.Application.Models;

public sealed record GeoAnchorInput(
    double Latitude,
    double Longitude,
    string Type);
