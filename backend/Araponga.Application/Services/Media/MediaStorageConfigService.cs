using Araponga.Application.Interfaces;
using Araponga.Application.Interfaces.Media;
using Araponga.Domain.Media;

namespace Araponga.Application.Services.Media;

/// <summary>
/// Service para gerenciar configurações de blob storage para mídias.
/// Permite configuração explícita e aberta do provedor (Local, S3, AzureBlob) via painel administrativo.
/// </summary>
public sealed class MediaStorageConfigService
{
    private readonly IMediaStorageConfigRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogger _auditLogger;

    public MediaStorageConfigService(
        IMediaStorageConfigRepository repository,
        IUnitOfWork unitOfWork,
        IAuditLogger auditLogger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _auditLogger = auditLogger;
    }

    /// <summary>
    /// Obtém a configuração ativa de storage.
    /// </summary>
    public async Task<MediaStorageConfig?> GetActiveConfigAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetActiveAsync(cancellationToken);
    }

    /// <summary>
    /// Lista todas as configurações de storage.
    /// </summary>
    public async Task<IReadOnlyList<MediaStorageConfig>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAllAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém configuração por ID.
    /// </summary>
    public async Task<MediaStorageConfig?> GetByIdAsync(Guid configId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(configId, cancellationToken);
    }

    /// <summary>
    /// Cria nova configuração de storage (inativa por padrão).
    /// </summary>
    public async Task<MediaStorageConfig> CreateConfigAsync(
        MediaStorageProvider provider,
        MediaStorageSettings settings,
        Guid createdByUserId,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        var config = new MediaStorageConfig(
            Guid.NewGuid(),
            provider,
            settings,
            isActive: false, // Nova configuração é inativa por padrão
            description,
            DateTime.UtcNow,
            createdByUserId,
            updatedAtUtc: null,
            updatedByUserId: null);

        await _repository.AddAsync(config, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        await _auditLogger.LogAsync(
            new Application.Models.AuditEntry(
                "media_storage_config.created",
                createdByUserId,
                Guid.Empty, // territoryId (configuração é global, não por território)
                config.Id,
                DateTime.UtcNow),
            cancellationToken);

        return config;
    }

    /// <summary>
    /// Atualiza configuração de storage existente.
    /// </summary>
    public async Task<MediaStorageConfig> UpdateConfigAsync(
        Guid configId,
        MediaStorageSettings settings,
        Guid updatedByUserId,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        var config = await _repository.GetByIdAsync(configId, cancellationToken);
        if (config is null)
        {
            throw new InvalidOperationException($"Storage config with ID {configId} not found.");
        }

        config.Update(settings, description, updatedByUserId, DateTime.UtcNow);

        await _repository.UpdateAsync(config, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        await _auditLogger.LogAsync(
            new Application.Models.AuditEntry(
                "media_storage_config.updated",
                updatedByUserId,
                Guid.Empty, // territoryId (configuração é global, não por território)
                config.Id,
                DateTime.UtcNow),
            cancellationToken);

        return config;
    }

    /// <summary>
    /// Ativa uma configuração de storage (desativa todas as outras).
    /// </summary>
    public async Task<MediaStorageConfig> ActivateConfigAsync(
        Guid configId,
        Guid updatedByUserId,
        CancellationToken cancellationToken = default)
    {
        var config = await _repository.GetByIdAsync(configId, cancellationToken);
        if (config is null)
        {
            throw new InvalidOperationException($"Storage config with ID {configId} not found.");
        }

        // Desativar todas as configurações ativas antes de ativar a nova
        var allConfigs = await _repository.ListAllAsync(cancellationToken);
        var activeConfigs = allConfigs.Where(c => c.IsActive && c.Id != configId).ToList();
        foreach (var activeConfig in activeConfigs)
        {
            activeConfig.Deactivate(updatedByUserId, DateTime.UtcNow);
            await _repository.UpdateAsync(activeConfig, cancellationToken);
        }

        // Ativar a configuração selecionada
        config.Activate(updatedByUserId, DateTime.UtcNow);

        await _repository.UpdateAsync(config, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        await _auditLogger.LogAsync(
            new Application.Models.AuditEntry(
                "media_storage_config.activated",
                updatedByUserId,
                Guid.Empty, // territoryId (configuração é global, não por território)
                config.Id,
                DateTime.UtcNow),
            cancellationToken);

        return config;
    }
}
