using Arah.Modules.Marketplace.Domain;

namespace Arah.Application.Models;

public sealed record InquiryCreationResult(
    ItemInquiry Inquiry,
    StoreContactInfo Contact);
