using MediatR;

namespace MiniETicaret.Application.Features.Queries.Cart.GetCartItems;

public class GetCartItemsQueryRequest : IRequest<List<GetCartItemsQueryResponse>>
{
}