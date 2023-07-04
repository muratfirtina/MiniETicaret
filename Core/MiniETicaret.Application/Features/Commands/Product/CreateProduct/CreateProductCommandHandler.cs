using FluentValidation;
using MediatR;
using MiniETicaret.Application.Abstractions.Hubs;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.ViewModels.Products;

namespace MiniETicaret.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandHandler:IRequestHandler<CreateProductCommandRequest,CreateProductCommandResponse>
{
    readonly IProductWriteRepository _productWriteRepository;
    readonly IProductHubService _productHubService;
    readonly IValidator<VM_Create_Product> _validator;

    public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService, IValidator<VM_Create_Product> validator)
    {
        _productWriteRepository = productWriteRepository;
        _productHubService = productHubService;
        _validator = validator;
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
        await _productWriteRepository.AddAsync(new()
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        });
        await _productWriteRepository.SaveAsync();
        await _productHubService.ProductAddedMessageAsync($"{request.Name} added.");
        return new();
    }
}
