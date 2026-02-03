namespace Araponga.Api.Contracts.Journeys.Marketplace;

public sealed record CheckoutJourneyRequest(
    Guid TerritoryId,
    string? PaymentMethod,
    AddressDto? ShippingAddress,
    string? Message);

public sealed record AddressDto(
    string? Street,
    string? Number,
    string? Complement,
    string? Neighborhood,
    string? City,
    string? State,
    string? ZipCode,
    string? Country);
