using Arah.Modules.Moderation.Application.Interfaces;
using Arah.Modules.Moderation.Domain.Evidence;

namespace Arah.Infrastructure.InMemory;

public sealed class InMemoryDocumentEvidenceRepository : IDocumentEvidenceRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryDocumentEvidenceRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<DocumentEvidence?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var found = _dataStore.DocumentEvidences.FirstOrDefault(e => e.Id == id);
        return Task.FromResult<DocumentEvidence?>(found);
    }

    public Task AddAsync(DocumentEvidence evidence, CancellationToken cancellationToken)
    {
        _dataStore.DocumentEvidences.Add(evidence);
        return Task.CompletedTask;
    }
}

