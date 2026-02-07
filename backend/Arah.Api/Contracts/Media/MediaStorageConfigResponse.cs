using Arah.Domain.Media;

namespace Arah.Api.Contracts.Media;

/// <summary>
/// Resposta da configuração de blob storage para mídias.
/// </summary>
public sealed record MediaStorageConfigResponse(
    Guid Id,
    string Provider,
    MediaStorageSettingsResponse Settings,
    bool IsActive,
    string? Description,
    DateTime CreatedAtUtc,
    Guid CreatedByUserId,
    DateTime? UpdatedAtUtc,
    Guid? UpdatedByUserId);

/// <summary>
/// Configurações de storage para mídias (response).
/// </summary>
public sealed record MediaStorageSettingsResponse(
    bool EnableUrlCache,
    TimeSpan? UrlCacheExpiration,
    LocalStorageSettingsResponse? Local,
    S3StorageSettingsResponse? S3,
    AzureBlobStorageSettingsResponse? AzureBlob);

/// <summary>
/// Configurações para armazenamento local (response).
/// </summary>
public sealed record LocalStorageSettingsResponse(string BasePath);

/// <summary>
/// Configurações para Amazon S3 (response).
/// </summary>
public sealed record S3StorageSettingsResponse(
    string BucketName,
    string Region,
    string AccessKeyId, // Mascarado para segurança (mostrar apenas últimos 4 caracteres)
    string? Prefix);

/// <summary>
/// Configurações para Azure Blob Storage (response).
/// </summary>
public sealed record AzureBlobStorageSettingsResponse(
    string ConnectionString, // Mascarado para segurança (mostrar apenas host)
    string ContainerName,
    string? Prefix);
