using ASMC5.Models;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ASMC5.data
{
    public class ASMDBContext : IdentityDbContext<User,Position,Guid>
    {
        public ASMDBContext()
        {

        }
        public ASMDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDestination> PaymentDestinations { get; set; }
        public DbSet<PaymentNotification> PaymentNotifications { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<PaymentSignature> PaymentSignatures { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<BillDetail> billDetails { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartDetail> cartDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Token> tokens { get; set; }
        public DbSet<UploadResult> Uploads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());      
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-VP99U9L\\SQLEXPRESS;Database=DB_CS5;User Id=sql; Password=19112002; TrustServerCertificate=True;");
            }
        }
    }
}
