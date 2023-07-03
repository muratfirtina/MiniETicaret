using MediatR;

namespace MiniETicaret.Application.Features.Commands.Cart.UpdateCartItem;

public class UpdateCartItemCommandRequest:IRequest<UpdateCartItemCommandResponse>
{
    public string CartItemId { get; set; }
    public bool IsChecked { get; set; }=true;
}