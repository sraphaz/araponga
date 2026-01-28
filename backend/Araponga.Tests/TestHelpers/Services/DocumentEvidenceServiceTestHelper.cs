using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Domain.Evidence;
using Araponga.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Tests.TestHelpers.Services;

public static class DocumentEvidenceServiceTestHelper
{
    public static DocumentEvidenceService CreateService(
        InMemoryDataStore dataStore,
        IFileStorage? fileStorage = null,
        IUnitOfWork? unitOfWork = null)
    {
        var services = new ServiceCollection();
        services.AddSingleton(dataStore);
        services.AddScoped<IDocumentEvidenceRepository, InMemoryDocumentEvidenceRepository>();
        services.AddScoped<IAuditLogger, InMemoryAuditLogger>();
        services.AddScoped<IUnitOfWork>(_ => unitOfWork ?? new InMemoryUnitOfWork());
        services.AddScoped<IFileStorage>(_ => fileStorage ?? new FakeFileStorage());
        services.AddScoped<DocumentEvidenceService>();
        return services.BuildServiceProvider().GetRequiredService<DocumentEvidenceService>();
    }

    private sealed class FakeFileStorage : IFileStorage
    {
        public StorageProvider Provider => StorageProvider.Local;

        public Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();
            content.CopyTo(ms);
            return Task.FromResult($"fake/{Guid.NewGuid():N}-{fileName}");
        }

        public Task<Stream> OpenReadAsync(string storageKey, CancellationToken cancellationToken)
        {
            return Task.FromResult<Stream>(new MemoryStream(new byte[] { 1, 2, 3 }));
        }
    }
}
