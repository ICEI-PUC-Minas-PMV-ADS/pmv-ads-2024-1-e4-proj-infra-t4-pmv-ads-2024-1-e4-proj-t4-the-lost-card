namespace Application.Contracts.LostCardDatabase;

public interface ILostCardDbUnitOfWork
{
    IPlayerRepository PlayerRepository { get; }
    IGameRoomRepository GameRoomRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
