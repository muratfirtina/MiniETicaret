using Microsoft.Extensions.DependencyInjection;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Abstractions.Services.Configurations;
using MiniETicaret.Application.Abstractions.Storage;
using MiniETicaret.Application.Abstractions.Storage.Local;
using MiniETicaret.Application.Abstractions.Token;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Infrastructure.Enums;
using MiniETicaret.Infrastructure.Services;
using MiniETicaret.Infrastructure.Services.Configurations;
using MiniETicaret.Infrastructure.Services.Storage;
using MiniETicaret.Infrastructure.Services.Storage.Azure;
using MiniETicaret.Infrastructure.Services.Storage.Local;
using MiniETicaret.Infrastructure.Services.Token;

namespace MiniETicaret.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IStorageService, StorageService>();
        serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        serviceCollection.AddScoped<IMailService, MailService>();
        serviceCollection.AddScoped<IApplicationService, ApplicationService>();
        serviceCollection.AddScoped<IQrCodeService, QrCodeService>();
        
        
        
    }
    public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : FileService, IStorage
    {
        serviceCollection.AddScoped<IStorage, T>();
    }
    public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
    {
        switch (storageType)
        {
            case StorageType.Local:
                serviceCollection.AddScoped<IStorage, LocalStorage>();
                break;
            case StorageType.Azure:
                serviceCollection.AddScoped<IStorage, AzureStorage>();
                break;
            case StorageType.AWS:
                //serviceCollection.AddScoped<IStorage, AWSStorage>();
                break;
            default:
                serviceCollection.AddScoped<IStorage, LocalStorage>();
                break;
        }
    }
    
}