using FluentValidation;
using MediatR;
using MiniETicaret.Application.Abstractions.Hubs;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.ViewModels.Products;

namespace MiniETicaret.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandHandler:IRequestHandler<CreateProductCommandRequest,CreateProductCommandResponse>
{
    readonly IProductService _productService;
    readonly IProductHubService _productHubService;
    readonly IValidator<VM_Create_Product> _validator;

    public CreateProductCommandHandler(IProductHubService productHubService, IValidator<VM_Create_Product> validator, IProductService productService)
    {
        
        _productHubService = productHubService;
        _validator = validator;
        _productService = productService;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(new(){Name=request.Name,Stock=request.Stock,Price=request.Price});
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
            throw new ArgumentException(string.Join(Environment.NewLine, validationErrors));
        }
        await _productService.CreateProductAsync(new()
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        });
        await _productHubService.ProductAddedMessageAsync($"{request.Name} added.");
        return new();
        
    }
}
