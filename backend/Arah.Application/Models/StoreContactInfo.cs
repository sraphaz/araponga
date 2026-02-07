using Arah.Modules.Marketplace.Domain;

namespace Arah.Application.Models;

public sealed record StoreContactInfo(
    StoreContactVisibility ContactVisibility,
    string? Phone,
    string? Whatsapp,
    string? Email,
    string? Instagram,
    string? Website,
    string? PreferredContactMethod);
