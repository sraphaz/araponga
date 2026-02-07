using Arah.Domain.Social.JoinRequests;

namespace Arah.Application.Models;

public sealed record ResidencyRequestResult(
    bool Created,
    TerritoryJoinRequest JoinRequest);

