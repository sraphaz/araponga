namespace Arah.Application.Models;

public sealed record PasswordResetTokenPayload(
    Guid UserId,
    DateTime ExpiresAtUtc);
