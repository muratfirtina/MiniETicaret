using Microsoft.Extensions.DependencyInjection;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.Services;
using MiniETicaret.Infrastructure.Services;

namespace MiniETicaret.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFileService, FileService>();
    }
}