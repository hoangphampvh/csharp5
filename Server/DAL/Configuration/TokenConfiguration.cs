using ASMC5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASMC5.Configuration
{
    public class TokenConfiguration : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.User).WithMany(p => p.Token).HasForeignKey(p => p.IdUser).HasConstraintName("FK_Token");
            builder.Property(p => p.Id).HasDefaultValueSql("(newid())");
            
        }
    }
}
