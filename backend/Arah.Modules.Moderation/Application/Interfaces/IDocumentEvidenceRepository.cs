using Arah.Modules.Moderation.Domain.Evidence;

namespace Arah.Modules.Moderation.Application.Interfaces;

public interface IDocumentEvidenceRepository
{
    Task<DocumentEvidence?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(DocumentEvidence evidence, CancellationToken cancellationToken);
}
