using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.LostCardDatabase.Mappings;

public class AchievmentsMapping : IEntityTypeConfiguration<Achievements>
{
    public void Configure(EntityTypeBuilder<Achievements> builder)
    {
        builder.HasKey(x => x.Id);
        builder.pa
        builder.Property(x => x.Name);
        builder.Property(x => x.Descriptions);
    }
}
