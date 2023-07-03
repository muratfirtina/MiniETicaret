using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Cart.AddItemToCart;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommandRequest, AddItemToCartCommandResponse>
{
    readonly ICartService _cartService;

    public AddItemToCartCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<AddItemToCartCommandResponse> Handle(AddItemToCartCommandRequest request, CancellationToken cancellationToken)
    {
        await _cartService.AddItemToCartAsync(new()
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            IsChecked = request.IsChecked
        });
        return new();
    }
    
}