namespace Araponga.Domain.Marketplace;

/// <summary>
/// Configuração de pagamento específica para um território.
/// Define qual gateway usar, credenciais, e configurações de transparência.
/// </summary>
public sealed class TerritoryPaymentConfig
{
    /// <summary>
    /// Inicializa uma nova instância de TerritoryPaymentConfig.
    /// </summary>
    /// <param name="id">Identificador único da configuração</param>
    /// <param name="territoryId">Identificador do território</param>
    /// <param name="gatewayProvider">Provedor do gateway (ex: "stripe", "mercadopago", "pagseguro")</param>
    /// <param name="isActive">Indica se a configuração está ativa</param>
    /// <param name="currency">Moeda padrão (ex: BRL, USD)</param>
    /// <param name="minimumAmount">Valor mínimo para transações (em centavos)</param>
    /// <param name="maximumAmount">Valor máximo para transações (em centavos, null = sem limite)</param>
    /// <param name="showFeeBreakdown">Se true, mostra breakdown de fees para o usuário</param>
    /// <param name="feeTransparencyLevel">Nível de transparência de fees (Basic, Detailed, Full)</param>
    /// <param name="createdAtUtc">Data de criação</param>
    /// <param name="updatedAtUtc">Data da última atualização</param>
    public TerritoryPaymentConfig(
        Guid id,
        Guid territoryId,
        string gatewayProvider,
        bool isActive,
        string currency,
        long minimumAmount,
        long? maximumAmount,
        bool showFeeBreakdown,
        FeeTransparencyLevel feeTransparencyLevel,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        if (territoryId == Guid.Empty)
        {
            throw new ArgumentException("Territory ID is required.", nameof(territoryId));
        }

        if (string.IsNullOrWhiteSpace(gatewayProvider))
        {
            throw new ArgumentException("Gateway provider is required.", nameof(gatewayProvider));
        }

        if (minimumAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumAmount), "Minimum amount must be non-negative.");
        }

        if (maximumAmount.HasValue && maximumAmount.Value < minimumAmount)
        {
            throw new ArgumentException("Maximum amount must be greater than or equal to minimum amount.");
        }

        Id = id;
        TerritoryId = territoryId;
        GatewayProvider = gatewayProvider;
        IsActive = isActive;
        Currency = currency;
        MinimumAmount = minimumAmount;
        MaximumAmount = maximumAmount;
        ShowFeeBreakdown = showFeeBreakdown;
        FeeTransparencyLevel = feeTransparencyLevel;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    /// <summary>
    /// Identificador único da configuração.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Identificador do território.
    /// </summary>
    public Guid TerritoryId { get; }

    /// <summary>
    /// Provedor do gateway de pagamento (ex: "stripe", "mercadopago", "pagseguro").
    /// </summary>
    public string GatewayProvider { get; private set; }

    /// <summary>
    /// Indica se a configuração está ativa e pode ser usada.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Moeda padrão para transações (ex: BRL, USD).
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// Valor mínimo para transações (em centavos ou menor unidade da moeda).
    /// </summary>
    public long MinimumAmount { get; private set; }

    /// <summary>
    /// Valor máximo para transações (em centavos, null = sem limite).
    /// </summary>
    public long? MaximumAmount { get; private set; }

    /// <summary>
    /// Se true, mostra breakdown detalhado de fees para o usuário antes do pagamento.
    /// </summary>
    public bool ShowFeeBreakdown { get; private set; }

    /// <summary>
    /// Nível de transparência de fees exibido ao usuário.
    /// </summary>
    public FeeTransparencyLevel FeeTransparencyLevel { get; private set; }

    /// <summary>
    /// Data de criação da configuração.
    /// </summary>
    public DateTime CreatedAtUtc { get; }

    /// <summary>
    /// Data da última atualização da configuração.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Atualiza a configuração de pagamento.
    /// </summary>
    public void Update(
        string gatewayProvider,
        bool isActive,
        string currency,
        long minimumAmount,
        long? maximumAmount,
        bool showFeeBreakdown,
        FeeTransparencyLevel feeTransparencyLevel,
        DateTime updatedAtUtc)
    {
        if (string.IsNullOrWhiteSpace(gatewayProvider))
        {
            throw new ArgumentException("Gateway provider is required.", nameof(gatewayProvider));
        }

        if (minimumAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumAmount), "Minimum amount must be non-negative.");
        }

        if (maximumAmount.HasValue && maximumAmount.Value < minimumAmount)
        {
            throw new ArgumentException("Maximum amount must be greater than or equal to minimum amount.");
        }

        GatewayProvider = gatewayProvider;
        IsActive = isActive;
        Currency = currency;
        MinimumAmount = minimumAmount;
        MaximumAmount = maximumAmount;
        ShowFeeBreakdown = showFeeBreakdown;
        FeeTransparencyLevel = feeTransparencyLevel;
        UpdatedAtUtc = updatedAtUtc;
    }

    /// <summary>
    /// Valida se um valor está dentro dos limites configurados.
    /// </summary>
    public bool IsAmountValid(long amountInCents)
    {
        if (amountInCents < MinimumAmount)
        {
            return false;
        }

        if (MaximumAmount.HasValue && amountInCents > MaximumAmount.Value)
        {
            return false;
        }

        return true;
    }
}

/// <summary>
/// Nível de transparência de fees exibido ao usuário.
/// </summary>
public enum FeeTransparencyLevel
{
    /// <summary>
    /// Básico: mostra apenas o valor total (fees incluídas).
    /// </summary>
    Basic = 1,

    /// <summary>
    /// Detalhado: mostra subtotal, fees e total separadamente.
    /// </summary>
    Detailed = 2,

    /// <summary>
    /// Completo: mostra breakdown completo incluindo percentuais e valores fixos.
    /// </summary>
    Full = 3
}
