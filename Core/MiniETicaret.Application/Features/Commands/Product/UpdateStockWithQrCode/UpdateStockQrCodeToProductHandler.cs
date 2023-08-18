using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Product.UpdateStockWithQrCode;

public class UpdateStockQrCodeToProductHandler: IRequestHandler<UpdateStockQrCodeToProductRequest, UpdateStockQrCodeToProductResponse>
{
    readonly IProductService _productService;

    public UpdateStockQrCodeToProductHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<UpdateStockQrCodeToProductResponse> Handle(UpdateStockQrCodeToProductRequest request, CancellationToken cancellationToken)
    {
        await _productService.StockUpdateWithQrCodeAsync(request.ProductId, request.Stock);
        return new();
    }
}