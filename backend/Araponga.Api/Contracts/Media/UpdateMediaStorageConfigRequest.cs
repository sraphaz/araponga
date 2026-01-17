using System.ComponentModel.DataAnnotations;

namespace Araponga.Api.Contracts.Media;

/// <summary>
/// Request para atualizar configuração de blob storage para mídias.
/// </summary>
public sealed record UpdateMediaStorageConfigRequest
{
    public MediaStorageSettingsRequest? Settings { get; init; }

    [MaxLength(1000)]
    public string? Description { get; init; }
}
