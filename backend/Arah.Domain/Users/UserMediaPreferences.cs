namespace Arah.Domain.Users;

/// <summary>
/// Preferências de mídia do usuário, permitindo escolher quais tipos de mídia visualizar.
/// </summary>
public sealed class UserMediaPreferences
{
    public Guid UserId { get; set; }

    /// <summary>
    /// Indica se o usuário deseja visualizar imagens.
    /// </summary>
    public bool ShowImages { get; set; } = true;

    /// <summary>
    /// Indica se o usuário deseja visualizar vídeos.
    /// </summary>
    public bool ShowVideos { get; set; } = true;

    /// <summary>
    /// Indica se o usuário deseja visualizar áudios.
    /// </summary>
    public bool ShowAudio { get; set; } = true;

    /// <summary>
    /// Indica se vídeos devem ser reproduzidos automaticamente.
    /// </summary>
    public bool AutoPlayVideos { get; set; } = false;

    /// <summary>
    /// Indica se áudios devem ser reproduzidos automaticamente.
    /// </summary>
    public bool AutoPlayAudio { get; set; } = false;

    /// <summary>
    /// Data/hora UTC da última atualização.
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}
