using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.LostCardDatabase.Mappings;

public class PlayerMapping : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasPartitionKey(c => c.PartitionKey);
        builder.Property(x => x.Name);
        builder.Property(x => x.PasswordSalt);
        builder.Property(x => x.PasswordHash);
        builder.Property(x => x.Email);
        builder.Property(x => x.CurrentRoom);
        builder.Property(x => x.JoinedRoomAt);
        builder.Property(x => x.Progrees);
        builder.HasMany<Achievements>(x => x.Achivements);
    }
}
