using Microsoft.EntityFrameworkCore;
using TaskPracticeOrder.Models;
using TaskPracticeOrder.Models.ViewModels;

namespace TaskPracticeOrder.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public DbSet<UnitItem> UnitItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<UnitItem>(entity =>
            {
                // modelBuilder.Entity<UnitItem>().HasKey(sc => new { sc.UnitId, sc.ItemId });
                entity.ToTable("UnitItem");
               
               
                entity.HasOne(d => d.Item)
                    .WithMany(p => p.UnitItems)
                    .HasForeignKey(d => d.ItemId);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.UnitItems)
                    .HasForeignKey(d => d.UnitId);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");

                entity.HasOne(d => d.UnitItem)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ItemUnitID);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderItemID);
            });
        }



    }
}
