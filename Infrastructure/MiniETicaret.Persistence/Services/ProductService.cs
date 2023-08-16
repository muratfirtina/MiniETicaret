using System.Text.Json;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Services;

public class ProductService : IProductService
{
    readonly IProductReadRepository _productReadRepository;
    readonly IQrCodeService _qrCodeService;

    public ProductService(IProductReadRepository productReadRepository, IQrCodeService qrCodeService)
    {
        _productReadRepository = productReadRepository;
        _qrCodeService = qrCodeService;
    }

    public async Task<byte[]> QrCodeToProductAsync(string productId)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        if (product is null)
            throw new Exception("Product not found.");
        
        var plainObject = new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.CreatedDate
        };
        string plainObjectJson = JsonSerializer.Serialize(plainObject);
        return _qrCodeService.GenerateQrCode(plainObjectJson);

    }
}