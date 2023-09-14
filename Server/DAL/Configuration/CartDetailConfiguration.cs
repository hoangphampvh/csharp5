using ASMC5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace assiment_csad4.Configruration
{
    public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.Property(x => x.ID).HasDefaultValueSql("(newid())");
            builder.ToTable("CartDetail");
            builder.HasKey(x => x.ID);
            builder.HasOne(x => x.Cart).WithMany(x => x.CartDetails).HasForeignKey(x => x.UserID);
            builder.HasOne(x => x.Product).WithMany(x => x.CartDetails).HasForeignKey(x => x.ProductID);
            builder.Property(p => p.Quantity).HasColumnType("int").IsRequired().HasDefaultValueSql("1");

        }
    }
}
