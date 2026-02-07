using Arah.Modules.Moderation.Domain.Evidence;

namespace Arah.Application.Interfaces;

public interface IFileStorage
{
    StorageProvider Provider { get; }

    Task<string> SaveAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken);

    Task<Stream> OpenReadAsync(
        string storageKey,
        CancellationToken cancellationToken);
}

