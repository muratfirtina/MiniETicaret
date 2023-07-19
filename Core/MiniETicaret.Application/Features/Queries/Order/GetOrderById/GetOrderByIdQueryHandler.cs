using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Queries.Order.GetOrderById;

public class GetOrderByIdQueryHandler: IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
{
    readonly IOrderService _orderService;

    public GetOrderByIdQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
    {
       var data = await _orderService.GetOrderByIdAsync(request.Id);
       return new ()
       {
           Id = data.Id,
           Address = data.Address,
           CartItems = data.CartItems,
           Description = data.Description,
           CreatedDate = data.CreatedDate,
           OrderCode = data.OrderCode,
           Completed = data.Completed
           
       };

    }
}