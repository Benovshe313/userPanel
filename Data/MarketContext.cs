
using user_panel2.Models;
using Microsoft.EntityFrameworkCore;

namespace admin_panel2.Data
{
    public class MarketContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-RFGQSC7;Initial Catalog=MarketApp;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Bought> Boughts { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bought>().HasKey(b => b.Id);
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal");

            modelBuilder.Entity<Product>().HasMany(p => p.Baskets).WithOne(b => b.Product)
                .HasForeignKey(b => b.ProductId);

            modelBuilder.Entity<Basket>().HasOne(b => b.User).WithMany(u => u.Baskets)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Basket>().HasOne(b => b.Product).WithMany().HasForeignKey(b => b.ProductId);
        }
    }
}


