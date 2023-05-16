using ASMC5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.Description).HasColumnType("nvarchar(256)");
            builder.HasOne(x => x.User).WithOne(x=>x.CartNavigation).HasForeignKey<Cart>(p => p.UserId);

        }
    }
}
