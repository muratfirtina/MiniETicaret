using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Order;
using MiniETicaret.Application.Repositories;

namespace MiniETicaret.Persistence.Services;

public class OrderService: IOrderService
{
    readonly IOrderWriteRepository _orderWriteRepository;

    public OrderService(IOrderWriteRepository orderWriteRepository)
    {
        _orderWriteRepository = orderWriteRepository;
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        await _orderWriteRepository.AddAsync(new()
        {
            Id = Guid.Parse(createOrder.CartId),
            Description = createOrder.Description,
            Address = createOrder.Address

        });
        await _orderWriteRepository.SaveAsync();
    }
}