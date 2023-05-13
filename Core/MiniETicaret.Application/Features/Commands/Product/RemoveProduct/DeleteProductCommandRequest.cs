using MediatR;

namespace MiniETicaret.Application.Features.Commands.Product.RemoveProduct;

public class DeleteProductCommandRequest: IRequest<DeleteProductCommandResponse>
{
    public string Id { get; set; }
}
