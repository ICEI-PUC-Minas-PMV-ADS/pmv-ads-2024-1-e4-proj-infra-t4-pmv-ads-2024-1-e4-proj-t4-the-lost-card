using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LostCardDatabase.Mappings;

public class GameRoomMapping : IEntityTypeConfiguration<GameRoom>
{
    public void Configure(EntityTypeBuilder<GameRoom> builder)
    {
        builder.Property(x => x.IsInviteOnly);
        builder.Property(x => x.Name);
    }
}
