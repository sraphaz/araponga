namespace Araponga.Domain.Social;

/// <summary>
/// Define o nível de validação do vínculo residencial de um Membership.
/// Separado do MembershipRole para eliminar ambiguidades entre papel e verificação.
/// </summary>
public enum ResidencyVerification
{
    /// <summary>
    /// Sem verificação de residência.
    /// </summary>
    Unverified = 1,

    /// <summary>
    /// Verificado por geolocalização.
    /// </summary>
    GeoVerified = 2,

    /// <summary>
    /// Verificado por comprovante documental.
    /// </summary>
    DocumentVerified = 3
}
