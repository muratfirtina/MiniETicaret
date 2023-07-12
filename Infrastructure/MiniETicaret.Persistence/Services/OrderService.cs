using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Order;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Services;

public class OrderService: IOrderService
{
    readonly IOrderWriteRepository _orderWriteRepository;
    readonly IOrderReadRepository _orderReadRepository;
    readonly ICartService _cartService;

    public OrderService(IOrderWriteRepository orderWriteRepository, ICartService cartService, IOrderReadRepository orderReadRepository)
    {
        _orderWriteRepository = orderWriteRepository;
        _cartService = cartService;
        _orderReadRepository = orderReadRepository;
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        Cart? cart = await _cartService.GetUserActiveCart();

        // Seçili olan ürünleri siparişe bağla
        var selectedCartItems = cart?.CartItems.Where(ci => ci.IsChecked).ToList();
        if (selectedCartItems != null && selectedCartItems.Count == 0)
        {
            throw new Exception("Sipariş vermek için seçili ürün bulunamadı.");
        }

        // Yeni bir sipariş oluştur
        var orderCode = (new Random().NextDouble() * 10000).ToString(CultureInfo.InvariantCulture);
        orderCode = orderCode.Substring(orderCode.IndexOf(".", StringComparison.Ordinal) + 1, orderCode.Length - orderCode.IndexOf(".", StringComparison.Ordinal) - 1);
        Order newOrder = new Order
        {
            Id = Guid.Parse(createOrder.CartId),
            OrderCode = orderCode,
            Description = createOrder.Description,
            Address = createOrder.Address,
            Cart = cart
        };

        // Yeni bir kart oluştur seçili olmayan ürünler için
        var unselectedCartItems = cart?.CartItems.Where(ci => !ci.IsChecked).ToList();
        if (unselectedCartItems != null && unselectedCartItems.Count > 0)
        {
            Cart newCart = new Cart
            {
                UserId = cart.UserId,
                User = cart.User,
                CartItems = unselectedCartItems
            };

            foreach (var cartItem in newCart.CartItems)
            {
                cartItem.Cart = newCart; // Yeni kart referansını ayarla
            }

            cart.User.Carts.Add(newCart); // Yeni kartı kullanıcıya ekle
        }

        // Seçili ürünleri siparişe bağla
        if (selectedCartItems != null)
            foreach (var cartItem in selectedCartItems)
            {
                cartItem.IsChecked = true; // Seçili ürünlerin IsChecked değerini true yap
                newOrder.Cart?.CartItems.Add(cartItem);
            }

        // Siparişi kaydet
        await _orderWriteRepository.AddAsync(newOrder);
        await _orderWriteRepository.SaveAsync();
    }

    public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
    {
        var query = _orderReadRepository.Table.Include(o => o.Cart)
            .ThenInclude(c => c.User)
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product);



        var data = query.OrderBy(o=>o.CreatedDate).Skip(page * size).Take(size);
        
        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data.Select(o=> new 
            {
                Id = o.Id,
                OrderCode = o.OrderCode,
                UserName = o.Cart.User.UserName,
                TotalPrice = o.Cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                CreatedDate = o.CreatedDate,
                
            }).ToListAsync()
        };

    }

    public async Task<SingleOrder> GetOrderByIdAsync(string id)
    {
        var data = await _orderReadRepository.Table
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));
        return new()
        {
            Id = data.Id.ToString(),
            OrderCode = data.OrderCode,
            Address = data.Address,
            Description = data.Description,
            CreatedDate = data.CreatedDate,
            CartItems = data.Cart.CartItems.Select(ci => new
            {
                ci.Product.Name,
                ci.Product.Price,
                ci.Quantity,
            })
        };
        
    }
}