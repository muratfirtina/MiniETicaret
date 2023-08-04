using MiniETicaret.Application.DTOs.Order;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrder createOrder);
    Task<ListOrder> GetAllOrdersAsync(int page, int size);
    Task<SingleOrder> GetOrderByIdAsync(string id);
    Task<(bool,CompleteOrderDto)> CompleteOrderAsync(string id);
}