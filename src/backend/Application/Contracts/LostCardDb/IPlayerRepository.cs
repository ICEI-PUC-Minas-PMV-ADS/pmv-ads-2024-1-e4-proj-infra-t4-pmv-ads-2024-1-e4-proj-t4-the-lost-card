using Domain.Entities;

namespace Application.Contracts.LostCardDb;

public interface IPlayerRepository
{
    Task<Player?> Find(int id, CancellationToken cancellationToken = default);
    Task<Player?> Find(string email, CancellationToken cancellationToken = default);
    Task Create(Player player, CancellationToken cancellationToken = default);
    void Update(Player player);
}
