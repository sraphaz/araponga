using Microsoft.EntityFrameworkCore;

namespace Araponga.Infrastructure.Postgres;

/// <summary>
/// DbContext somente leitura para usar read replicas.
/// Configurado para usar connection string de read replica e NoTracking por padrão.
/// </summary>
public sealed class ReadOnlyArapongaDbContext : ArapongaDbContext
{
    public ReadOnlyArapongaDbContext(DbContextOptions<ArapongaDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Read-only context cannot save changes. Use the main ArapongaDbContext for write operations.");
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("Read-only context cannot save changes. Use the main ArapongaDbContext for write operations.");
    }
}
