using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Order;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Services;

public class OrderService: IOrderService
{
    readonly IOrderWriteRepository _orderWriteRepository;
    readonly ICartService _cartService;

    public OrderService(IOrderWriteRepository orderWriteRepository, ICartService cartService)
    {
        _orderWriteRepository = orderWriteRepository;
        _cartService = cartService;
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
        Order newOrder = new Order
        {
            Id = Guid.Parse(createOrder.CartId),
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


}