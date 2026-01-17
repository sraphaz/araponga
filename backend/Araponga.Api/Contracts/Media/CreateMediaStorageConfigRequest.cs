using System.ComponentModel.DataAnnotations;

namespace Araponga.Api.Contracts.Media;

/// <summary>
/// Request para criar nova configuração de blob storage para mídias.
/// </summary>
public sealed record CreateMediaStorageConfigRequest
{
    [Required]
    public string Provider { get; init; } = null!; // "Local", "S3", "AzureBlob"

    public MediaStorageSettingsRequest? Settings { get; init; }

    [MaxLength(1000)]
    public string? Description { get; init; }
}

/// <summary>
/// Configurações de storage para mídias (request).
/// </summary>
public sealed record MediaStorageSettingsRequest
{
    public bool? EnableUrlCache { get; init; }
    public TimeSpan? UrlCacheExpiration { get; init; }
    public LocalStorageSettingsRequest? Local { get; init; }
    public S3StorageSettingsRequest? S3 { get; init; }
    public AzureBlobStorageSettingsRequest? AzureBlob { get; init; }
}

/// <summary>
/// Configurações para armazenamento local (request).
/// </summary>
public sealed record LocalStorageSettingsRequest
{
    [Required]
    public string BasePath { get; init; } = null!;
}

/// <summary>
/// Configurações para Amazon S3 (request).
/// </summary>
public sealed record S3StorageSettingsRequest
{
    [Required]
    public string BucketName { get; init; } = null!;

    [Required]
    public string Region { get; init; } = null!;

    [Required]
    public string AccessKeyId { get; init; } = null!;

    public string? Prefix { get; init; }
}

/// <summary>
/// Configurações para Azure Blob Storage (request).
/// </summary>
public sealed record AzureBlobStorageSettingsRequest
{
    [Required]
    public string ConnectionString { get; init; } = null!;

    [Required]
    public string ContainerName { get; init; } = null!;

    public string? Prefix { get; init; }
}
