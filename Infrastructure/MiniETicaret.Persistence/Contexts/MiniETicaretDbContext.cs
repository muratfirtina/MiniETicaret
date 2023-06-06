using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Domain.Entities.Common;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Contexts;

public class MiniETicaretDbContext : IdentityDbContext<AppUser,AppRole,string>
{
    public MiniETicaretDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Domain.Entities.File> Files { get; set; }
    public DbSet<ProductImageFile> ProductImageFiles { get; set; }
    public DbSet<InvoiceFile> InvoiceFiles { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Order>()
            .HasKey(o => o.Id);
        
        builder.Entity<Cart>()
            .HasOne(c => c.Order)
            .WithOne(o => o.Cart)
            .HasForeignKey<Order>(o => o.Id);
        
        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {   
        
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            _ = entry.State switch
            {
                EntityState.Added => entry.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => entry.Entity.UpdatedDate = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
    
    
}
