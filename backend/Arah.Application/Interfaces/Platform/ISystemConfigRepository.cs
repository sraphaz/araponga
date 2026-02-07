using Arah.Domain.Configuration;

namespace Arah.Application.Interfaces;

public interface ISystemConfigRepository
{
    Task<SystemConfig?> GetByKeyAsync(string key, CancellationToken cancellationToken);
    Task<IReadOnlyList<SystemConfig>> ListAsync(SystemConfigCategory? category, CancellationToken cancellationToken);
    Task UpsertAsync(SystemConfig config, CancellationToken cancellationToken);
}

