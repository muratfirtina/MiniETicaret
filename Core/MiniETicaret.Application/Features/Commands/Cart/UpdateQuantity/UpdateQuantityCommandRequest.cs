using MediatR;

namespace MiniETicaret.Application.Features.Commands.Cart.UpdateQuantity;

public class UpdateQuantityCommandRequest : IRequest<UpdateQuantityCommandResponse>
{
    public string CartItemId { get; set; }
    public int Quantity { get; set; }
    
}