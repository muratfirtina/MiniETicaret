using MiniETicaret.Application.DTOs.Product;
using MiniETicaret.Application.ViewModels.Products;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IProductService
{
    Task<ListProductDto> GetAllProductsAsync(int page, int size);
    Task<ProductDto> CreateProductAsync(VM_Create_Product createProductmodel);
    Task<ProductDto> UpdateProductAsync(VM_Update_Product updateProductModel);
    //satışa göre ürün stoğunu güncelle
    Task<ProductStockDto> UpdateProductOrderStockAsync(string productId, int stock);
    Task<int>GetProductStockAsync(string productId);
    Task<ProductDto> GetProductByIdAsync(string productId);
    
    Task<byte[]> QrCodeToProductAsync(string productId);
    Task StockUpdateWithQrCodeAsync(string productId, int stock);
}