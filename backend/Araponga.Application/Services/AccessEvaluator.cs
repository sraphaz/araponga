using Araponga.Application.Interfaces;
using Araponga.Domain.Social;
using Araponga.Domain.Users;

namespace Araponga.Application.Services;

public sealed class AccessEvaluator
{
    private readonly ITerritoryMembershipRepository _membershipRepository;

    public AccessEvaluator(ITerritoryMembershipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

    public async Task<bool> IsResidentAsync(Guid userId, Guid territoryId, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepository.GetByUserAndTerritoryAsync(userId, territoryId, cancellationToken);
        return membership is not null &&
               membership.Role == MembershipRole.Resident &&
               membership.VerificationStatus == VerificationStatus.Validated;
    }

    public async Task<MembershipRole?> GetRoleAsync(Guid userId, Guid territoryId, CancellationToken cancellationToken)
    {
        var membership = await _membershipRepository.GetByUserAndTerritoryAsync(userId, territoryId, cancellationToken);
        return membership?.Role;
    }

    public bool IsCurator(User user)
    {
        return user.Role == UserRole.Curator;
    }
}
