using ASMC5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
     
            builder.ToTable("Role");
        
            builder.Property(x => x.status).HasColumnType("int").HasDefaultValueSql("((0))").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(256)").IsRequired();
            builder.HasIndex(p => p.Name); // Unique
        }
    }
}
