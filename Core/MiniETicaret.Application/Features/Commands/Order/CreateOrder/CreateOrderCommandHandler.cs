using MediatR;
using MiniETicaret.Application.Abstractions.Hubs;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Order.CreateOrder;

public class CreateOrderCommandHandler: IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
{
    readonly IOrderService _orderService;
    readonly ICartService _cartService;
    readonly IOrderHubService _orderHubService;

    public CreateOrderCommandHandler(IOrderService orderService, ICartService cartService, IOrderHubService orderHubService)
    {
        _orderService = orderService;
        _cartService = cartService;
        _orderHubService = orderHubService;
    }

    public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await _orderService.CreateOrderAsync(new()
        {
            Description = request.Description,
            Address = request.Address,
            CartId = _cartService.GetUserActiveCart().Result.Id.ToString()
        });
        await _orderHubService.OrderAddedMessageAsync("New Order Added");
        return new();
    }
}