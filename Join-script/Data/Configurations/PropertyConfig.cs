using Join_script.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Join_script.Data.Configurations;
public class PropertyConfig : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasOne(_ => _.PropertyType)
            .WithMany(_ => _.Properties)
            .HasForeignKey(_ => _.TypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
