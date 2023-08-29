using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Order.RemoveOrderItem;

public class RemoveOrderItemCommandHandler: IRequestHandler<RemoveOrderItemCommandRequest, RemoveOrderItemCommandResponse>
{
    readonly IOrderService _orderService;

    public RemoveOrderItemCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<RemoveOrderItemCommandResponse> Handle(RemoveOrderItemCommandRequest request, CancellationToken cancellationToken)
    {
        await _orderService.RemoveOrderItemAsync(request.CartItemId);
        return new();
    }
}