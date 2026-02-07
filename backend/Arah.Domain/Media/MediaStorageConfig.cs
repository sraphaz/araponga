namespace Arah.Domain.Media;

/// <summary>
/// Configuração de provedor de blob storage para mídias, gerenciada por SystemAdmin no painel administrativo.
/// Permite configuração explícita e aberta do provedor (Local, S3, AzureBlob) ao invés de fixo via appsettings.json.
/// </summary>
public sealed class MediaStorageConfig
{
    public const int MaxDescriptionLength = 1000;

    public MediaStorageConfig(
        Guid id,
        MediaStorageProvider provider,
        MediaStorageSettings settings,
        bool isActive,
        string? description,
        DateTime createdAtUtc,
        Guid createdByUserId,
        DateTime? updatedAtUtc,
        Guid? updatedByUserId)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("ID is required.", nameof(id));
        }

        if (createdByUserId == Guid.Empty)
        {
            throw new ArgumentException("CreatedByUserId is required.", nameof(createdByUserId));
        }

        var normalizedDescription = NormalizeOptional(description);
        if (normalizedDescription is not null && normalizedDescription.Length > MaxDescriptionLength)
        {
            throw new ArgumentException($"Description must not exceed {MaxDescriptionLength} characters.", nameof(description));
        }

        Id = id;
        Provider = provider;
        Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        IsActive = isActive;
        Description = normalizedDescription;
        CreatedAtUtc = createdAtUtc;
        CreatedByUserId = createdByUserId;
        UpdatedAtUtc = updatedAtUtc;
        UpdatedByUserId = updatedByUserId;
    }

    public Guid Id { get; }
    public MediaStorageProvider Provider { get; }
    public MediaStorageSettings Settings { get; private set; }
    public bool IsActive { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public Guid CreatedByUserId { get; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public Guid? UpdatedByUserId { get; private set; }

    public void Update(
        MediaStorageSettings settings,
        string? description,
        Guid updatedByUserId,
        DateTime updatedAtUtc)
    {
        if (updatedByUserId == Guid.Empty)
        {
            throw new ArgumentException("UpdatedByUserId is required.", nameof(updatedByUserId));
        }

        var normalizedDescription = NormalizeOptional(description);
        if (normalizedDescription is not null && normalizedDescription.Length > MaxDescriptionLength)
        {
            throw new ArgumentException($"Description must not exceed {MaxDescriptionLength} characters.", nameof(description));
        }

        Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        Description = normalizedDescription;
        UpdatedAtUtc = updatedAtUtc;
        UpdatedByUserId = updatedByUserId;
    }

    public void Activate(Guid updatedByUserId, DateTime updatedAtUtc)
    {
        if (updatedByUserId == Guid.Empty)
        {
            throw new ArgumentException("UpdatedByUserId is required.", nameof(updatedByUserId));
        }

        IsActive = true;
        UpdatedAtUtc = updatedAtUtc;
        UpdatedByUserId = updatedByUserId;
    }

    public void Deactivate(Guid updatedByUserId, DateTime updatedAtUtc)
    {
        if (updatedByUserId == Guid.Empty)
        {
            throw new ArgumentException("UpdatedByUserId is required.", nameof(updatedByUserId));
        }

        IsActive = false;
        UpdatedAtUtc = updatedAtUtc;
        UpdatedByUserId = updatedByUserId;
    }

    private static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

/// <summary>
/// Provedores de blob storage suportados para mídias.
/// </summary>
public enum MediaStorageProvider
{
    /// <summary>
    /// Armazenamento local em disco (desenvolvimento/testes).
    /// </summary>
    Local = 1,

    /// <summary>
    /// Amazon S3 (produção recomendado).
    /// </summary>
    S3 = 2,

    /// <summary>
    /// Azure Blob Storage (produção).
    /// </summary>
    AzureBlob = 3
}

/// <summary>
/// Configurações de storage para mídias.
/// </summary>
public sealed record MediaStorageSettings
{
    /// <summary>
    /// Configurações comuns a todos os providers.
    /// </summary>
    public bool EnableUrlCache { get; init; } = true;
    public TimeSpan? UrlCacheExpiration { get; init; } = TimeSpan.FromHours(24);

    /// <summary>
    /// Configurações específicas por provider.
    /// </summary>
    public LocalStorageSettings? Local { get; init; }
    public S3StorageSettings? S3 { get; init; }
    public AzureBlobStorageSettings? AzureBlob { get; init; }
}

/// <summary>
/// Configurações para armazenamento local.
/// </summary>
public sealed record LocalStorageSettings(string BasePath);

/// <summary>
/// Configurações para Amazon S3.
/// </summary>
public sealed record S3StorageSettings(
    string BucketName,
    string Region,
    string AccessKeyId,
    string? Prefix = null);

/// <summary>
/// Configurações para Azure Blob Storage.
/// </summary>
public sealed record AzureBlobStorageSettings(
    string ConnectionString,
    string ContainerName,
    string? Prefix = null);
