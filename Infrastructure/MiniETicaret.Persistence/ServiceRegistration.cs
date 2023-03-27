using Microsoft.Extensions.DependencyInjection;
using MiniETicaret.Application.Abstractions;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<IProductService, ProductService>();
    }
}