namespace Arah.Application.Models;

/// <summary>
/// Resultado do setup de 2FA, contendo o secret e o QR code para configuração.
/// </summary>
public sealed class TwoFactorSetupResult
{
    public TwoFactorSetupResult(string secret, string qrCodeUri, string manualEntryKey)
    {
        Secret = secret;
        QrCodeUri = qrCodeUri;
        ManualEntryKey = manualEntryKey;
    }

    public string Secret { get; }
    public string QrCodeUri { get; }
    public string ManualEntryKey { get; }
}

/// <summary>
/// Resultado da confirmação de 2FA, contendo os recovery codes.
/// </summary>
public sealed class TwoFactorConfirmResult
{
    public TwoFactorConfirmResult(IReadOnlyList<string> recoveryCodes)
    {
        RecoveryCodes = recoveryCodes;
    }

    public IReadOnlyList<string> RecoveryCodes { get; }
}

/// <summary>
/// Resultado do login quando 2FA está habilitado.
/// </summary>
public sealed class TwoFactorRequiredResult
{
    public TwoFactorRequiredResult(string challengeId)
    {
        ChallengeId = challengeId;
    }

    public string ChallengeId { get; }
}
