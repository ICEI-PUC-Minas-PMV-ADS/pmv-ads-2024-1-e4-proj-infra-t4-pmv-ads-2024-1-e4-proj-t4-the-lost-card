using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Infrastructure.LostCardDatabase.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase;

public class LostCardDbContext : DbContext, ILostCardDbUnitOfWork
{
    public LostCardDbContext(DbContextOptions<LostCardDbContext> options) : base(options)
    {

    }

    public DbSet<Player> Players { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerMapping());

        base.OnModelCreating(modelBuilder);
    }
}
