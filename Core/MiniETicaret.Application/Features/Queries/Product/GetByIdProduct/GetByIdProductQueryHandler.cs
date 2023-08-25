using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Repositories;
using P = MiniETicaret.Domain.Entities;

namespace MiniETicaret.Application.Features.Queries.Product.GetByIdProduct;

public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
{

    readonly IProductService _productService;
    public GetByIdProductQueryHandler(IProductService productService)
    {
        _productService = productService;
        
    }

    public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductByIdAsync(request.Id);
        return new GetByIdProductQueryResponse
        {
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}