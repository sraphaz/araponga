using Arah.Application.Models;

namespace Arah.Application.Interfaces;

public interface IAuditLogger
{
    Task LogAsync(AuditEntry entry, CancellationToken cancellationToken);
}
