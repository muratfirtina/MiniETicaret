using MediatR;

namespace MiniETicaret.Application.Features.Commands.Product.UpdateStockWithQrCode;

public class UpdateStockQrCodeToProductRequest: IRequest<UpdateStockQrCodeToProductResponse>
{
    public string ProductId { get; set; }
    public int Stock { get; set; }
}