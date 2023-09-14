using ASMC5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            
            builder.ToTable("User");
         
            builder.Property(x => x.Status).HasColumnType("int").HasDefaultValueSql("((0))").IsRequired();
            builder.Property(x => x.Password).HasColumnType("nvarchar(256)").IsRequired();
          
            // khoa ngoai trong bang cart tro toi bang user
            builder.HasMany(x => x.Bill).WithOne(x => x.User).HasForeignKey(p => p.UserID);         
            
        }
    }
}
