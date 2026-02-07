using Arah.Application.Models;

namespace Arah.Application.Interfaces;

public interface IChatConversationStatsRepository
{
    Task<ChatConversationStats?> GetAsync(Guid conversationId, CancellationToken cancellationToken);
    Task UpsertAsync(ChatConversationStats stats, CancellationToken cancellationToken);
}
