using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase.Repositories;

internal class GameRoomRepository : IGameRoomRepository
{
    private readonly LostCardDbContext lostCardDbContext;

    public GameRoomRepository(LostCardDbContext lostCardDbContext)
    {
        this.lostCardDbContext = lostCardDbContext;
    }

    public async Task Create(GameRoom gameRoom, CancellationToken cancellationToken = default)
    {
        await lostCardDbContext.AddAsync(gameRoom, cancellationToken);
    }

    public Task<GameRoom?> Find(Guid id, CancellationToken cancellation = default)
    {
        return lostCardDbContext.GameRooms.FirstOrDefaultAsync(gr => gr.Guid == id, cancellation);
    }

    public async Task<IEnumerable<GameRoom>> Find(CancellationToken cancellation = default)
    {
        return await lostCardDbContext.GameRooms.ToArrayAsync(cancellation);
    }

    public void Remove(GameRoom gameRoom)
    {
        lostCardDbContext.Remove(gameRoom);
    }

    public void Update(GameRoom gameRoom)
    {
        lostCardDbContext.Update(gameRoom);
    }
}
