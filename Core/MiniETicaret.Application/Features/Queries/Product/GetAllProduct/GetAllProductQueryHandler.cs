using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Repositories;

namespace MiniETicaret.Application.Features.Queries.Product.GetAllProduct;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    readonly IProductService _productService;
    //readonly ILogger<GetAllProductQueryHandler> _logger;

    public GetAllProductQueryHandler(ILogger<GetAllProductQueryHandler> logger, IProductService productService)
    {
        _productService = productService;
        
        //_logger = logger;
    }

    public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
       // _logger.LogInformation("Get All Product");
        
       var data = await _productService.GetAllProductsAsync(request.Page, request.Size);
       return new()
       {
           Products = data.Products,
           TotalProductCount = data.TotalProductCount
       };
    }
}
