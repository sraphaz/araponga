using Araponga.Application.Common;
using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Araponga.Domain.Marketplace;

namespace Araponga.Application.Services;

/// <summary>
/// Serviço para gerenciar configurações de pagamento por território.
/// Garante economia justa e transparente através de configurações claras de fees e limites.
/// </summary>
public sealed class TerritoryPaymentConfigService
{
    private readonly ITerritoryPaymentConfigRepository _configRepository;
    private readonly IPlatformFeeConfigRepository _platformFeeRepository;
    private readonly IFeatureFlagService _featureFlagService;
    private readonly IUnitOfWork _unitOfWork;

    public TerritoryPaymentConfigService(
        ITerritoryPaymentConfigRepository configRepository,
        IPlatformFeeConfigRepository platformFeeRepository,
        IFeatureFlagService featureFlagService,
        IUnitOfWork unitOfWork)
    {
        _configRepository = configRepository;
        _platformFeeRepository = platformFeeRepository;
        _featureFlagService = featureFlagService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Verifica se pagamentos estão habilitados para um território.
    /// Requer feature flag PaymentEnabled E configuração ativa.
    /// </summary>
    public async Task<bool> IsPaymentEnabledAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        // Verificar feature flag primeiro
        if (!_featureFlagService.IsEnabled(territoryId, FeatureFlag.PaymentEnabled))
        {
            return false;
        }

        // Verificar se há configuração ativa
        var config = await _configRepository.GetActiveAsync(territoryId, cancellationToken);
        return config is not null && config.IsActive;
    }

    /// <summary>
    /// Obtém a configuração ativa de pagamento para um território.
    /// </summary>
    public async Task<Result<TerritoryPaymentConfig>> GetActiveConfigAsync(
        Guid territoryId,
        CancellationToken cancellationToken)
    {
        var config = await _configRepository.GetActiveAsync(territoryId, cancellationToken);
        if (config is null)
        {
            return Result<TerritoryPaymentConfig>.Failure("Payment configuration not found or not active for this territory.");
        }

        return Result<TerritoryPaymentConfig>.Success(config);
    }

    /// <summary>
    /// Cria ou atualiza a configuração de pagamento para um território.
    /// </summary>
    public async Task<Result<TerritoryPaymentConfig>> UpsertConfigAsync(
        Guid territoryId,
        Guid actorUserId,
        string gatewayProvider,
        bool isActive,
        string currency,
        long minimumAmount,
        long? maximumAmount,
        bool showFeeBreakdown,
        FeeTransparencyLevel feeTransparencyLevel,
        CancellationToken cancellationToken)
    {
        // Verificar se feature flag está habilitada
        if (!_featureFlagService.IsEnabled(territoryId, FeatureFlag.PaymentEnabled))
        {
            return Result<TerritoryPaymentConfig>.Failure(
                "Payment feature flag must be enabled for this territory before configuring payment settings.");
        }

        var existing = await _configRepository.GetActiveAsync(territoryId, cancellationToken);
        var now = DateTime.UtcNow;

        if (existing is not null)
        {
            // Atualizar existente
            existing.Update(
                gatewayProvider,
                isActive,
                currency,
                minimumAmount,
                maximumAmount,
                showFeeBreakdown,
                feeTransparencyLevel,
                now);

            await _configRepository.UpdateAsync(existing, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<TerritoryPaymentConfig>.Success(existing);
        }
        else
        {
            // Criar novo
            var newConfig = new TerritoryPaymentConfig(
                Guid.NewGuid(),
                territoryId,
                gatewayProvider,
                isActive,
                currency,
                minimumAmount,
                maximumAmount,
                showFeeBreakdown,
                feeTransparencyLevel,
                now,
                now);

            await _configRepository.AddAsync(newConfig, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<TerritoryPaymentConfig>.Success(newConfig);
        }
    }

    /// <summary>
    /// Lista todas as configurações de um território.
    /// </summary>
    public async Task<IReadOnlyList<TerritoryPaymentConfig>> ListConfigsAsync(
        Guid territoryId,
        CancellationToken cancellationToken)
    {
        return await _configRepository.ListByTerritoryAsync(territoryId, cancellationToken);
    }

    /// <summary>
    /// Valida se um valor está dentro dos limites configurados para o território.
    /// </summary>
    public async Task<Result<bool>> ValidateAmountAsync(
        Guid territoryId,
        long amountInCents,
        CancellationToken cancellationToken)
    {
        var configResult = await GetActiveConfigAsync(territoryId, cancellationToken);
        if (configResult.IsFailure)
        {
            return Result<bool>.Failure(configResult.Error ?? "Payment configuration not found.");
        }

        var config = configResult.Value!;
        if (!config.IsAmountValid(amountInCents))
        {
            var minFormatted = FormatAmount(config.MinimumAmount, config.Currency);
            var maxFormatted = config.MaximumAmount.HasValue
                ? FormatAmount(config.MaximumAmount.Value, config.Currency)
                : "sem limite";

            return Result<bool>.Failure(
                $"Amount {FormatAmount(amountInCents, config.Currency)} is outside valid range: " +
                $"minimum {minFormatted}, maximum {maxFormatted}.");
        }

        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Calcula o breakdown de fees para transparência.
    /// </summary>
    public async Task<Result<FeeBreakdown>> CalculateFeeBreakdownAsync(
        Guid territoryId,
        long amountInCents,
        ItemType itemType,
        CancellationToken cancellationToken)
    {
        var configResult = await GetActiveConfigAsync(territoryId, cancellationToken);
        if (configResult.IsFailure)
        {
            return Result<FeeBreakdown>.Failure(configResult.Error ?? "Payment configuration not found.");
        }

        var config = configResult.Value!;

        // Buscar configuração de fee da plataforma para o tipo de item
        var feeConfig = await _platformFeeRepository.GetActiveAsync(territoryId, itemType, cancellationToken);
        
        decimal platformFeePercent = 0m;
        decimal platformFeeFixed = 0m;

        if (feeConfig is not null && feeConfig.IsActive)
        {
            if (feeConfig.FeeMode == PlatformFeeMode.Percentage)
            {
                platformFeePercent = feeConfig.FeeValue;
            }
            else if (feeConfig.FeeMode == PlatformFeeMode.Fixed)
            {
                platformFeeFixed = feeConfig.FeeValue;
            }
        }

        var subtotal = amountInCents;
        var platformFee = (long)(subtotal * platformFeePercent / 100) + (long)(platformFeeFixed * 100); // Converter fixed para centavos
        var total = subtotal + platformFee;

        var breakdown = new FeeBreakdown(
            subtotal,
            platformFee,
            total,
            config.Currency,
            platformFeePercent,
            platformFeeFixed,
            config.ShowFeeBreakdown,
            config.FeeTransparencyLevel);

        return Result<FeeBreakdown>.Success(breakdown);
    }

    private static string FormatAmount(long amountInCents, string currency)
    {
        var amount = amountInCents / 100.0m;
        return $"{currency} {amount:F2}";
    }
}

/// <summary>
/// Breakdown de fees para transparência.
/// </summary>
public sealed record FeeBreakdown(
    long SubtotalInCents,
    long PlatformFeeInCents,
    long TotalInCents,
    string Currency,
    decimal PlatformFeePercent,
    decimal PlatformFeeFixed,
    bool ShowBreakdown,
    FeeTransparencyLevel TransparencyLevel);
