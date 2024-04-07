using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Domain.Services;
using Infrastructure.LostCardDatabase.Mappings;
using Infrastructure.LostCardDatabase.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase;

public class LostCardDbContext : DbContext, ILostCardDbUnitOfWork
{
    private readonly IDateTimeService dateTimeService;
    public LostCardDbContext(DbContextOptions<LostCardDbContext> options, IDateTimeService dateTimeService) : base(options)
    {
        PlayerRepository = new PlayerRepository(this);
        this.dateTimeService = dateTimeService;
    }

    public DbSet<Player> Players { get; set; } = null!;

    public IPlayerRepository PlayerRepository { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerMapping());

        base.OnModelCreating(modelBuilder);
    }
}
