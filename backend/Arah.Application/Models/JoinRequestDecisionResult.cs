using Arah.Domain.Social.JoinRequests;

namespace Arah.Application.Models;

public sealed record JoinRequestDecisionResult(
    bool Found,
    bool Forbidden,
    TerritoryJoinRequest? Request,
    bool Updated);
