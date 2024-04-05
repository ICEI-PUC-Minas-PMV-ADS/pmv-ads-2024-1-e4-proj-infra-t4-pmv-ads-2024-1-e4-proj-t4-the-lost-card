using Domain.Entities;

namespace Application.Contracts.LostCardDatabase;

public interface IPlayerRepository
{
    Task<Player?> Find(string id, CancellationToken cancellationToken = default);
    Task<Player?> FindByEmail(string email, CancellationToken cancellationToken = default);
    Task Create(Player player, CancellationToken cancellationToken = default);
    void Update(Player player);
}
