using MediatR;
using MiniETicaret.Application.Features.Commands.ProductImageFile.UploadProductImage;

namespace MiniETicaret.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandRequest: IRequest<RemoveProductImageCommandResponse>
{
    public string Id { get; set; }
    public string? ImageId { get; set; }
}