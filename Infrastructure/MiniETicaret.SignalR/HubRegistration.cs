using Microsoft.AspNetCore.Builder;
using MiniETicaret.SignalR.Hubs;

namespace MiniETicaret.SignalR;

public static class HubRegistration
{
    public static void MapHubs(this WebApplication webApplication)
    {
        webApplication.MapHub<ProductHub>("/products-hub");
    }
}