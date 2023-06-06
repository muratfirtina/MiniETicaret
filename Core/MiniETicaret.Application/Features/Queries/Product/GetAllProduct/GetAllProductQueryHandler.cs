using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniETicaret.Application.Repositories;

namespace MiniETicaret.Application.Features.Queries.Product.GetAllProduct;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    readonly IProductReadRepository _productReadRepository;
    readonly ILogger<GetAllProductQueryHandler> _logger;

    public GetAllProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductQueryHandler> logger)
    {
        _productReadRepository = productReadRepository;
        _logger = logger;
    }

    public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get All Product");
        
        var totalProductCount = _productReadRepository.GetAll(false).Count();
        var products = await _productReadRepository.GetAll(false)
            .OrderBy(p => p.Id)
            .Skip(request.Page * request.Size)
            .Take(request.Size)
            .Include(p => p.ProductImageFiles)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImageFiles
            }).ToListAsync();
        return new()
        {
            TotalProductCount = totalProductCount,
            Products = products

        };
    }
}
