using Domain.Entities;

namespace Application.Contracts.LostCardDatabase;

public interface IGameRoomRepository
{
    Task Create(GameRoom gameRoom, CancellationToken cancellationToken = default);
    ValueTask<GameRoom?> Find(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GameRoom>> Find(IEnumerable<GameRoomState>? semaphoreStatesFilter = default, CancellationToken cancellationToken = default);
    Task Remove(GameRoom gameRoom, CancellationToken cancellationToken = default);
    Task Update(GameRoom gameRoom, CancellationToken cancellationToken = default);
}
