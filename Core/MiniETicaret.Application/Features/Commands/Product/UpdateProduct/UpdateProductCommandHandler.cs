using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.ViewModels.Products;

namespace MiniETicaret.Application.Features.Commands.Product.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    readonly IProductService _productService;
    readonly IValidator<VM_Update_Product> _validator;
    readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, ILogger<UpdateProductCommandHandler> logger, IProductService productService, IValidator<VM_Update_Product> validator)
    {
        _logger = logger;
        _productService = productService;
        _validator = validator;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(new()
        {
            Id = request.Id,
            Name = request.Name,
            Stock = request.Stock,
            Price = request.Price
        });
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
            throw new ArgumentException(string.Join(Environment.NewLine, validationErrors));
        }

        await _productService.UpdateProductAsync(new()
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        });
        _logger.LogInformation($"{request.Name} updated.");
        return new();
        
    }
    
}

