using Araponga.Domain.Users;

namespace Araponga.Infrastructure.Postgres.Entities;

public sealed class UserRecord
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Cpf { get; set; }
    public string? ForeignDocument { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
