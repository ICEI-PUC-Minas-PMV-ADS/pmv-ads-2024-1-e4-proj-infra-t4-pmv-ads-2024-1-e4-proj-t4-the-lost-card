using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Infrastructure.LostCardDatabase.Mappings;
using Infrastructure.LostCardDatabase.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase;

public class LostCardDbContext : DbContext, ILostCardDbUnitOfWork
{
    public LostCardDbContext(DbContextOptions<LostCardDbContext> options) : base(options)
    {
        PlayerRepository = new PlayerRepository(this);
        GameRoomRepository = new GameRoomRepository(this);
    }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<GameRoom> GameRooms { get; set; } = null!;

    public IPlayerRepository PlayerRepository { get; init; }

    public IGameRoomRepository GameRoomRepository { get; init; }

    public DbSet<Achievements> Achievements { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerMapping());
        modelBuilder.ApplyConfiguration(new GameRoomMapping());

        base.OnModelCreating(modelBuilder);
    }
}
