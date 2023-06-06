using MediatR;

namespace MiniETicaret.Application.Features.Commands.Cart.RemoveCartItem;

public class RemoveCartItemCommandRequest : IRequest<RemoveCartItemCommandResponse>
{
    public string CartItemId { get; set; }
}