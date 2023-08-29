using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Order.RemoveOrder;

public class RemoveOrderCommandHandler: IRequestHandler<RemoveOrderCommandRequest, RemoveOrderCommandResponse>
{
    readonly IOrderService _orderService;

    public RemoveOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<RemoveOrderCommandResponse> Handle(RemoveOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await _orderService.DeleteOrder(request.Id);
        return new RemoveOrderCommandResponse();
        
    }
}