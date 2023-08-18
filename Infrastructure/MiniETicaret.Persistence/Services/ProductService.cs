using System.Text.Json;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Services;

public class ProductService : IProductService
{
    readonly IProductReadRepository _productReadRepository;
    readonly IProductWriteRepository _productWriteRepository;
    readonly IQrCodeService _qrCodeService;

    public ProductService(IProductReadRepository productReadRepository, IQrCodeService qrCodeService, IProductWriteRepository productWriteRepository)
    {
        _productReadRepository = productReadRepository;
        _qrCodeService = qrCodeService;
        _productWriteRepository = productWriteRepository;
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

    public async Task StockUpdateWithQrCodeAsync(string productId, int stock)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        if (product is null)
            throw new Exception("Product not found.");
        
        product.Stock = stock;
        await _productWriteRepository.SaveAsync();

    }
}