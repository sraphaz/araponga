namespace Arah.Infrastructure.Security;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "Arah";
    public string Audience { get; set; } = "Arah";
    public string SigningKey { get; set; } = "dev-only-change-me";
    public int ExpirationMinutes { get; set; } = 60;
}
