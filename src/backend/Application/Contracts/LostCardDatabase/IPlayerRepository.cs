using Domain.Entities;

namespace Application.Contracts.LostCardDatabase;

public interface IPlayerRepository
{
    ValueTask<Player?> Find(Guid id, CancellationToken cancellationToken = default);
    Task<Player?> Find(string email, CancellationToken cancellationToken = default);
    Task Create(Player player, CancellationToken cancellationToken = default);
    void Update(Player player);
}
