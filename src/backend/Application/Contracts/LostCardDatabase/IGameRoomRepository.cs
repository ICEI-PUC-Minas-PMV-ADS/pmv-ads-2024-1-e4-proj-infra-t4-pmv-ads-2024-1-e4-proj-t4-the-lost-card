using Domain.Entities;

namespace Application.Contracts.LostCardDatabase;

public interface IGameRoomRepository
{
    Task Create(GameRoom gameRoom, CancellationToken cancellationToken = default);
    Task<GameRoom?> Find(Guid id, CancellationToken cancellation = default);
    void Update(GameRoom gameRoom);
    void Remove(GameRoom gameRoom);
}
