namespace Araponga.Domain.Territories;

public sealed class Territory
{
    public Territory(
        Guid id,
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
    public string Name { get; }
    public string? Description { get; }
    public TerritoryStatus Status { get; }
    public string City { get; }
    public string State { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public DateTime CreatedAtUtc { get; }
}
