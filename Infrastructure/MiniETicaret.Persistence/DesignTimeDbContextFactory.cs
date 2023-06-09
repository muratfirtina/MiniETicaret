using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MiniETicaretDbContext>
{
    public MiniETicaretDbContext CreateDbContext(string[] args)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<MiniETicaretDbContext>();
        dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString).EnableSensitiveDataLogging();
        
        
        return new MiniETicaretDbContext(dbContextOptionsBuilder.Options);
    }
    
    
}