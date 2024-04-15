using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase.Mappings;

public class GameRoomMapping : IEntityTypeConfiguration<GameRoom>
{
    public void Configure(EntityTypeBuilder<GameRoom> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasPartitionKey(c => c.PartitionKey);
        builder.Property(x => x.IsInviteOnly);
        builder.Property(x => x.Name);
        builder.Property(gr => gr.AdminId);
        builder.Property(gr => gr.Semaphore);
        builder.OwnsMany(gr => gr.Players);
        builder.OwnsOne(gr => gr.GameInfo);
    }
}
