using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Order;

namespace MiniETicaret.Application.Features.Commands.Order.CompleteOrder;

public class CompleteOrderCommandHandler: IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
{
    IOrderService _orderService;
    IMailService _mailService;

    public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
    {
        _orderService = orderService;
        _mailService = mailService;
    }

    public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
    {
        (bool succeeded, CompleteOrderDto dto)  = await _orderService.CompleteOrderAsync(request.Id);
        if (succeeded)
            _mailService.SendCompletedOrderEmailAsync(dto.Email, dto.OrderCode, dto.OrderDescription, dto.OrderAddress, dto.OrderCreatedDate, dto.UserName, dto.OrderCartItems, dto.OrderTotalPrice);
        
        return new ();
    }
}