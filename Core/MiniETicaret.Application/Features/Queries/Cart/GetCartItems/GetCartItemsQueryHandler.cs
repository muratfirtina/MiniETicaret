using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Application.Features.Queries.Cart.GetCartItems;

public class GetCartItemsQueryHandler: IRequestHandler<GetCartItemsQueryRequest, List<GetCartItemsQueryResponse>>
{
    readonly ICartService _cartService;

    public GetCartItemsQueryHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<List<GetCartItemsQueryResponse>> Handle(GetCartItemsQueryRequest request, CancellationToken cancellationToken)
    {
        var cartItems = await _cartService.GetCartItemsAsync();
        return cartItems.Select(ci => new GetCartItemsQueryResponse
        {
            CartItemId = ci.Id.ToString(),
            ProductName = ci.Product.Name,
            Quantity = ci.Quantity,
            UnitPrice = ci.Product.Price,
            ProductImageUrls = ci.Product.ProductImageFiles.Select(pif => pif.Path).ToList(),
            IsChecked = ci.IsChecked
            
        }).ToList();
    }
}