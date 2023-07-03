using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.ViewModels.Carts;

namespace MiniETicaret.Application.Features.Commands.Cart.UpdateCartItem;

public class UpdateCartItemCommandHandler:IRequestHandler<UpdateCartItemCommandRequest,UpdateCartItemCommandResponse>
{
    readonly ICartService _cartService;

    public UpdateCartItemCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<UpdateCartItemCommandResponse> Handle(UpdateCartItemCommandRequest request, CancellationToken cancellationToken)
    {
        await _cartService.UpdateCartItemAsync(new VM_Update_CartItem()
        {
            CartItemId = request.CartItemId,
            IsChecked = request.IsChecked,
            
        });
        return new();
    }
}