using MediatR;

namespace MiniETicaret.Application.Features.Commands.Cart.AddItemToCart;

public class AddItemToCartCommandRequest : IRequest<AddItemToCartCommandResponse>
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public bool IsChecked { get; set; } = true;
}