using MiniETicaret.Application.DTOs.Order;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrder createOrder);
}