using Microsoft.Extensions.DependencyInjection;
using MiniETicaret.Application.Abstractions.Hubs;
using MiniETicaret.SignalR.HubServices;

namespace MiniETicaret.SignalR;

public static class ServiceRegistration
{
    public static void AddSignalRServices(this IServiceCollection collection)
    {
        collection.AddTransient<IProductHubService, ProductHubService>();
        collection.AddTransient<IOrderHubService, OrderHubService>();
        collection.AddSignalR();
    }
}