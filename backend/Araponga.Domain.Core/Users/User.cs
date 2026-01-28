namespace Araponga.Domain.Core.Users;

/// <summary>
/// Representa um usuário do sistema. Contém informações de identidade, autenticação e verificação.
/// </summary>
public sealed class User
{
    /// <summary>
    /// Cria uma nova instância de User com valores padrão para 2FA e verificação de identidade.
    /// </summary>
    public User(
        Guid id,
        string displayName,
        string? email,
        string? cpf,
        string? foreignDocument,
        string? phoneNumber,
        string? address,
        string authProvider,
        string externalId,
        DateTime createdAtUtc)
        : this(id, displayName, email, cpf, foreignDocument, phoneNumber, address, authProvider, externalId, false, null, null, null, UserIdentityVerificationStatus.Unverified, null, null, null, createdAtUtc)
    {
    }

    /// <summary>
    /// Cria uma nova instância de User com todos os parâmetros.
    /// </summary>
    public User(
        Guid id,
        string displayName,
        string? email,
        string? cpf,
        string? foreignDocument,
        string? phoneNumber,
        string? address,
        string authProvider,
        string externalId,
        bool twoFactorEnabled,
        string? twoFactorSecret,
        string? twoFactorRecoveryCodesHash,
        DateTime? twoFactorVerifiedAtUtc,
        UserIdentityVerificationStatus identityVerificationStatus,
        DateTime? identityVerifiedAtUtc,
        Guid? avatarMediaAssetId,
        string? bio,
        DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name is required.", nameof(displayName));
        }

        if (string.IsNullOrWhiteSpace(authProvider))
        {
            throw new ArgumentException("Auth provider is required.", nameof(authProvider));
        }

        if (string.IsNullOrWhiteSpace(externalId))
        {
            throw new ArgumentException("External ID is required.", nameof(externalId));
        }

        var normalizedCpf = NormalizeOptional(cpf);
        var normalizedForeignDocument = NormalizeOptional(foreignDocument);

        if (string.IsNullOrWhiteSpace(normalizedCpf) && string.IsNullOrWhiteSpace(normalizedForeignDocument))
        {
            throw new ArgumentException("CPF or foreign document is required.", nameof(cpf));
        }

        if (!string.IsNullOrWhiteSpace(normalizedCpf) && !string.IsNullOrWhiteSpace(normalizedForeignDocument))
        {
            throw new ArgumentException("Provide either CPF or foreign document, not both.", nameof(cpf));
        }

        Id = id;
        DisplayName = displayName.Trim();
        Email = NormalizeOptional(email);
        Cpf = normalizedCpf;
        ForeignDocument = normalizedForeignDocument;
        PhoneNumber = NormalizeOptional(phoneNumber);
        Address = NormalizeOptional(address);
        AuthProvider = authProvider.Trim();
        ExternalId = externalId.Trim();
        TwoFactorEnabled = twoFactorEnabled;
        TwoFactorSecret = twoFactorSecret;
        TwoFactorRecoveryCodesHash = twoFactorRecoveryCodesHash;
        TwoFactorVerifiedAtUtc = twoFactorVerifiedAtUtc;
        IdentityVerificationStatus = identityVerificationStatus;
        IdentityVerifiedAtUtc = identityVerifiedAtUtc;
        AvatarMediaAssetId = avatarMediaAssetId;
        Bio = NormalizeBio(bio);
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }
    public string DisplayName { get; }
    public string? Email { get; }
    public string? Cpf { get; }
    public string? ForeignDocument { get; }
    public string? PhoneNumber { get; }
    public string? Address { get; }
    public string AuthProvider { get; }
    public string ExternalId { get; }
    public bool TwoFactorEnabled { get; private set; }
    public string? TwoFactorSecret { get; private set; }
    public string? TwoFactorRecoveryCodesHash { get; private set; }
    public DateTime? TwoFactorVerifiedAtUtc { get; private set; }
    public UserIdentityVerificationStatus IdentityVerificationStatus { get; private set; }
    public DateTime? IdentityVerifiedAtUtc { get; private set; }
    public Guid? AvatarMediaAssetId { get; private set; }
    public string? Bio { get; private set; }
    public DateTime CreatedAtUtc { get; }

    public void EnableTwoFactor(string secret, string recoveryCodesHash, DateTime verifiedAtUtc)
    {
        TwoFactorEnabled = true;
        TwoFactorSecret = secret;
        TwoFactorRecoveryCodesHash = recoveryCodesHash;
        TwoFactorVerifiedAtUtc = verifiedAtUtc;
    }

    public void DisableTwoFactor()
    {
        TwoFactorEnabled = false;
        TwoFactorSecret = null;
        TwoFactorRecoveryCodesHash = null;
        TwoFactorVerifiedAtUtc = null;
    }

    public void UpdateIdentityVerification(UserIdentityVerificationStatus status, DateTime? verifiedAtUtc = null)
    {
        IdentityVerificationStatus = status;
        IdentityVerifiedAtUtc = verifiedAtUtc;
    }

    public void UpdateAvatar(Guid? avatarMediaAssetId)
    {
        AvatarMediaAssetId = avatarMediaAssetId;
    }

    public void UpdateBio(string? bio)
    {
        var normalized = NormalizeBio(bio);
        if (normalized is not null && normalized.Length > 500)
        {
            throw new ArgumentException("Bio must not exceed 500 characters.", nameof(bio));
        }
        Bio = normalized;
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? NormalizeBio(string? bio)
    {
        if (string.IsNullOrWhiteSpace(bio))
        {
            return null;
        }
        var trimmed = bio.Trim();
        return trimmed.Length == 0 ? null : trimmed;
    }
}
