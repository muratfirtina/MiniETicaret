using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Cart.UpdateQuantity;

public class UpdateQuantityCommandHandler: IRequestHandler<UpdateQuantityCommandRequest, UpdateQuantityCommandResponse>
{
    readonly ICartService _cartService;

    public UpdateQuantityCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<UpdateQuantityCommandResponse> Handle(UpdateQuantityCommandRequest request, CancellationToken cancellationToken)
    {
        await _cartService.UpdateQuantityAsync(new()
        {
            CartItemId = request.CartItemId,
            Quantity = request.Quantity

        });
        return new();
    }
}