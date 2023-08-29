using MiniETicaret.Application.DTOs.Order;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrder createOrder);
    Task<ListOrder> GetAllOrdersAsync(int page, int size);
    Task<SingleOrder> GetOrderByIdAsync(string id);
    Task<(bool,CompleteOrderDto)> CompleteOrderAsync(string id);
    Task<bool> RemoveOrderItemAsync(string? cartItemId);
    Task<bool> DeleteOrder(string id);
    
    //Task<ListOrder> GetOrdersByUserIdAsync(string userId, int page, int size);
    
}