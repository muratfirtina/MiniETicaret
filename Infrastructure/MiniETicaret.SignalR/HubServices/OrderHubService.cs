using Microsoft.AspNetCore.SignalR;
using MiniETicaret.Application.Abstractions.Hubs;
using MiniETicaret.SignalR.Hubs;

namespace MiniETicaret.SignalR.HubServices;

public class OrderHubService : IOrderHubService
{
    readonly IHubContext<OrderHub> _hubContext;

    public OrderHubService(IHubContext<OrderHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task OrderAddedMessageAsync(string message)
    {
        return _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage, message);
    }
}