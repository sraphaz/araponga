namespace Araponga.Api.Contracts.Auth;

public sealed record TwoFactorSetupResponse(
    string Secret,
    string QrCodeUri,
    string ManualEntryKey
);

public sealed record TwoFactorConfirmRequest(
    string Secret,
    string Code
);

public sealed record TwoFactorConfirmResponse(
    IReadOnlyList<string> RecoveryCodes
);

public sealed record TwoFactorVerifyRequest(
    string ChallengeId,
    string Code
);

public sealed record TwoFactorRecoverRequest(
    string ChallengeId,
    string RecoveryCode
);

public sealed record TwoFactorDisableRequest(
    string? Password,
    string? Code
);
