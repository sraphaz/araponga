using Arah.Domain.Users;

namespace Arah.Api.Security;

public sealed record UserContext(User? User, TokenStatus Status);
