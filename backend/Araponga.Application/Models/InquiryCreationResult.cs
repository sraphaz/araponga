using Araponga.Domain.Marketplace;

namespace Araponga.Application.Models;

public sealed record InquiryCreationResult(
    ItemInquiry Inquiry,
    StoreContactInfo Contact);
