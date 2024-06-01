using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Electronic_G5.Models
{
    public partial class ElectronicDb : DbContext
    {
        public ElectronicDb()
            : base("name=ElectronicDb")
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Order_Items> Order_Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Categories1)
                .WithOptional(e => e.Category1)
                .HasForeignKey(e => e.parent_id);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order_Items>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(e => e.total_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Order_Items)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Payment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductOption>()
                .Property(e => e.product_option_name)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOption>()
                .Property(e => e.product_option_value)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOption>()
                .Property(e => e.product_option_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.SKU)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.image_url)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Order_Items)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductOptions)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shipment>()
                .Property(e => e.zip_code)
                .IsUnicode(false);

            modelBuilder.Entity<Shipment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Shipment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.phone_number)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Shipments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
