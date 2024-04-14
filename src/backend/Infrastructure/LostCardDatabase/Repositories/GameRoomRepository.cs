﻿using Application.Contracts.LostCardDatabase;
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

    public ValueTask<GameRoom?> Find(Guid id, CancellationToken cancellationToken = default)
    {
        return lostCardDbContext.GameRooms.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<GameRoom>> Find(CancellationToken cancellationToken = default)
    {
        return await lostCardDbContext.GameRooms.ToArrayAsync(cancellationToken);
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
