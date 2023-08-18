namespace MiniETicaret.Application.Abstractions.Services;

public interface IProductService
{
    Task<byte[]> QrCodeToProductAsync(string productId);
    Task StockUpdateWithQrCodeAsync(string productId, int stock);
}