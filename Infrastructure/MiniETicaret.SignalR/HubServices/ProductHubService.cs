using Microsoft.AspNetCore.SignalR;
using MiniETicaret.Application.Abstractions.Hubs;
using MiniETicaret.SignalR.Hubs;

namespace MiniETicaret.SignalR.HubServices;

public class ProductHubService : IProductHubService
{
    readonly IHubContext<ProductHub> _productHubContext;

    public ProductHubService(IHubContext<ProductHub> productHubContext)
    {
        _productHubContext = productHubContext;
    }

    public async Task ProductAddedMessageAsync(string message)
    {
        await _productHubContext.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
    }
}