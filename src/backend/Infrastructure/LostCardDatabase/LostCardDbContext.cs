using Domain.Entities;
using Infrastructure.LostCardDatabase.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase;

public class LostCardDbContext : DbContext
{
    public LostCardDbContext(DbContextOptions<LostCardDbContext> options) : base(options)
    {

    }

    public DbSet<Player> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerMapping());

        base.OnModelCreating(modelBuilder);
    }
}
