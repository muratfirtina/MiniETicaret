using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Cart.RemoveCartItem;

public class RemoveCarItemCammandHandler: IRequestHandler<RemoveCartItemCommandRequest, RemoveCartItemCommandResponse>
{
    readonly ICartService _cartService;

    public RemoveCarItemCammandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<RemoveCartItemCommandResponse> Handle(RemoveCartItemCommandRequest request, CancellationToken cancellationToken)
    {
        await _cartService.RemoveCartItemAsync(request.CartItemId);
        return new();
    }
}