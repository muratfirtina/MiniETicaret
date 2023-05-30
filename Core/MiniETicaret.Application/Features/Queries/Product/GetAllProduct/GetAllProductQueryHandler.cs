using MediatR;
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
        var totalCount = _productReadRepository.GetAll(false).Count();
        var products = _productReadRepository.GetAll(false)
            .OrderBy(p => p.Id)
            .Skip(request.Page * request.Size)
            .Take(request.Size)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            }).ToList();
        return new()
        {
            TotalCount = totalCount,
            Products = products

        };
    }
}
