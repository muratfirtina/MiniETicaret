using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MiniETicaretDbContext>
{
    public MiniETicaretDbContext CreateDbContext(string[] args)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<MiniETicaretDbContext>();
        dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);

        return new MiniETicaretDbContext(dbContextOptionsBuilder.Options);
    }
}