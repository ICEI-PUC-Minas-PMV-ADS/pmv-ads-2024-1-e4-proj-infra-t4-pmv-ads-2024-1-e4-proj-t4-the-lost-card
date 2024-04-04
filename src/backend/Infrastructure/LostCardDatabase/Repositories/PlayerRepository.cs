using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly LostCardDbContext lostCardDbContext;

    public PlayerRepository(LostCardDbContext lostCardDbContext)
    {
        this.lostCardDbContext = lostCardDbContext;
    }

    public async Task Create(Player player, CancellationToken cancellationToken = default)
    {
        await lostCardDbContext.AddAsync(player, cancellationToken);
    }

    public Task<Player?> Find(string id, CancellationToken cancellationToken = default)
    {
        return lostCardDbContext.Players.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<Player?> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        return lostCardDbContext.Players.FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public void Update(Player player)
    {
        lostCardDbContext.Update(player);
    }
}
