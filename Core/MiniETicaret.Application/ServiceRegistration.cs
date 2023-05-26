using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MiniETicaret.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection collection)
    {
        collection.AddMediatR(typeof(ServiceRegistration));
        collection.AddHttpClient();
        
    }
}