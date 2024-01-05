using EzTech.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EzTech.Data;

public class EzTechDbContext : DbContext
{
    // Ignore warning taken from official docs on microsoft.com
    public EzTechDbContext(DbContextOptions<EzTechDbContext> options)
        : base(new DbContextOptionsBuilder<EzTechDbContext>(options)
            .ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId
                    .MultipleCollectionIncludeWarning))
            .Options)
    {
    }
    
    /// <summary>
    /// Used for migrations, is not used in production.
    /// It is used to create the database, and to update it.
    /// </summary>
    public class EzTechDbContextFactory : IDesignTimeDbContextFactory<EzTechDbContext>
    {
        public EzTechDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EzTechDbContext>();
            // Get connection string from user secrets
            optionsBuilder.UseMySql("server=localhost;user=root;password=Skejs123;database=eztechdb",
                new MySqlServerVersion(new Version(8, 0, 29)));
            return new EzTechDbContext(optionsBuilder.Options);
        }
    }
    

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Wishlist> Wishlists { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;
    public DbSet<Faq> Faqs { get; set; } = null!;
    public DbSet<WebsiteInfo> WebsiteInfos { get; set; } = null!;
    public DbSet<Promotion> Promotions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // USERS
        // indexes
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
        // relations
        modelBuilder.Entity<User>()
            .HasOne(e => e.Cart)
            .WithOne(e => e.User)
            .HasForeignKey<Cart>(e => e.UserId)
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasMany(e => e.Orders)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
        modelBuilder.Entity<User>()
            .HasOne(e => e.Wishlist)
            .WithOne(e => e.User)
            .HasForeignKey<Wishlist>(e => e.UserId)
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasMany(e => e.Ratings)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
        modelBuilder.Entity<Rating>()
            .HasOne(e => e.Product)
            .WithMany(e => e.Ratings)
            .HasForeignKey(e => e.ProductId);
        // PRODUCTS
        // indexes
        modelBuilder.Entity<Product>()
            .HasIndex(e => e.Name)
            .IsUnique();
        // relations
        modelBuilder.Entity<Product>()
            .HasMany(e => e.Categories)
            .WithMany(e => e.Products);
        modelBuilder.Entity<Product>()
            .OwnsMany(e => e.Images);
        modelBuilder.Entity<Product>()
            .HasMany(e => e.Ratings)
            .WithOne(e => e.Product)
            .HasForeignKey(e => e.ProductId);

        // CART
        modelBuilder.Entity<Cart>()
            .HasMany(e => e.CartItems)
            .WithOne(e => e.Cart)
            .HasForeignKey(e => e.CartId);
        modelBuilder.Entity<CartItem>()
            .HasOne(e => e.Product)
            .WithMany(e => e.CartItems)
            .HasForeignKey(e => e.ProductId);

        // WISHLIST
        modelBuilder.Entity<Wishlist>()
            .HasMany(e => e.Products)
            .WithMany(e => e.Wishlists);

        // ORDER
        // indexes
        modelBuilder.Entity<Order>()
            .HasIndex(e => e.OrderNumber)
            .IsUnique();
        // relations
        modelBuilder.Entity<Order>()
            .HasMany(e => e.Items)
            .WithOne(e => e.Order);
        modelBuilder.Entity<OrderItem>()
            .HasOne(e => e.Product)
            .WithMany(e => e.OrderItems)
            .HasForeignKey(e => e.ProductId);
        // FAQ
        modelBuilder.Entity<Faq>()
            .HasIndex(e => e.Question)
            .IsUnique();

        // WEBSITE INFO
        modelBuilder.Entity<WebsiteInfo>()
            .OwnsMany(e => e.WebsiteInfoFields);
        modelBuilder.Entity<WebsiteInfo>()
            .OwnsMany(e => e.WeeklyOpeningHours);
        modelBuilder.Entity<WebsiteInfo>()
            .OwnsMany(e => e.SpecialOpeningHours);
        // PROMOTION
        modelBuilder.Entity<Promotion>()
            .HasMany(e => e.Products)
            .WithMany(e => e.Promotions);

        modelBuilder.Entity<Promotion>()
            .HasIndex(e => e.Title)
            .IsUnique();
    }
}