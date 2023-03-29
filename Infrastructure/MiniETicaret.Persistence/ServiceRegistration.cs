using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniETicaret.Application.Abstractions;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<MiniETicaretDbContext>(options =>
            options.UseNpgsql(Configuration.ConnectionString));
        services.AddSingleton<IProductService, ProductService>();
    }
}