using MediatR;
using Microsoft.Extensions.Logging;
using MiniETicaret.Application.Repositories;

namespace MiniETicaret.Application.Features.Commands.Product.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    readonly IProductWriteRepository _productWriteRepository;
    readonly IProductReadRepository _productReadRepository;
    readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, ILogger<UpdateProductCommandHandler> logger)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
        _logger = logger;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        await _productWriteRepository.SaveAsync();
        _logger.LogInformation("Product updated successfully.");
        return new();
    }
    
}

