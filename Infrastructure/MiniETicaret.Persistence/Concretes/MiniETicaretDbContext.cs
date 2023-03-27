using Microsoft.EntityFrameworkCore;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Concretes;

public class MiniETicaretDbContext : DbContext
{
    public MiniETicaretDbContext(DbContextOptions<MiniETicaretDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
}
