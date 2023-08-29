using MediatR;

namespace MiniETicaret.Application.Features.Commands.Order.RemoveOrderItem;

public class RemoveOrderItemCommandRequest: IRequest<RemoveOrderItemCommandResponse>
{
    public string? CartItemId { get; set; }
}