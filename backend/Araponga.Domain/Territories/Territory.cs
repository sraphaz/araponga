namespace Araponga.Domain.Territories;

/// <summary>
/// Representa um território geográfico. Territory não possui Owner individual.
/// A governança é exercida por papéis territoriais atribuídos a memberships via TerritoryMembership.
/// </summary>
public sealed class Territory
{
    public Territory(
        Guid id,
        Guid? parentTerritoryId,
        string name,
        string? description,
        TerritoryStatus status,
        string city,
        string state,
        double latitude,
        double longitude,
        DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Territory name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City is required.", nameof(city));
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            throw new ArgumentException("State is required.", nameof(state));
        }

        Id = id;
        ParentTerritoryId = parentTerritoryId;
        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Status = status;
        City = city.Trim();
        State = state.Trim();
        Latitude = latitude;
        Longitude = longitude;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }
    public Guid? ParentTerritoryId { get; }
    public string Name { get; }
    public string? Description { get; }
    public TerritoryStatus Status { get; }
    public string City { get; }
    public string State { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public DateTime CreatedAtUtc { get; }
}
