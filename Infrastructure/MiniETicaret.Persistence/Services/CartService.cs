using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.ViewModels.Carts;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Persistence.Services;

public class CartService:ICartService

{
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly UserManager<AppUser> _userManager;
    readonly IOrderReadRepository _orderReadRepository;
    readonly ICartWriteRepository _cartWriteRepository;
    readonly ICartReadRepository _cartReadRepository;
    readonly ICartItemWriteRepository _cartItemWriteRepository;
    readonly ICartItemReadRepository _cartItemReadRepository;

    public CartService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, ICartWriteRepository cartWriteRepository, ICartItemWriteRepository cartItemWriteRepository, ICartItemReadRepository cartItemReadRepository, ICartReadRepository cartReadRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _orderReadRepository = orderReadRepository;
        _cartWriteRepository = cartWriteRepository;
        _cartItemWriteRepository = cartItemWriteRepository;
        _cartItemReadRepository = cartItemReadRepository;
        _cartReadRepository = cartReadRepository;
    }
    private async Task<Cart?> ContextUser()
    {
        var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userName))
        {
            AppUser? user = await _userManager.Users
                .Include(u =>u.Carts)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            var _cart = from cart in user.Carts
                join order in _orderReadRepository.Table 
                    on cart.Id equals order.Id into cartOrders
                from order in cartOrders.DefaultIfEmpty()
                select new
                {
                    Cart = cart,
                    Order = order
                };

            Cart? targetCart = null;
            if (_cart.Any(c =>c.Order is null))
            {
                targetCart = _cart.FirstOrDefault(c => c.Order is null)?.Cart;
            }
            else
            {
                targetCart = new();
                user.Carts.Add(targetCart);
            }
            
            await _cartWriteRepository.SaveAsync();
            return targetCart;

        }
        throw new Exception("Unexpected error occured.");
    }

    public async Task<List<CartItem>> GetCartItemsAsync()
    {
        Cart? cart = await ContextUser();
        Cart? result = await _cartReadRepository.Table
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p=>p.ProductImageFiles)
            .FirstOrDefaultAsync(c => c.Id == cart.Id);
        return result.CartItems
            .ToList();

    }

    public async Task AddItemToCartAsync(VM_Create_CartItem cartItem)
    {
        Cart? cart = await ContextUser();
        if (cart != null)
        {
            CartItem _cartItem = await _cartItemReadRepository
                .GetSingleAsync(ci => ci.CartId == cart.Id && ci.ProductId == Guid.Parse(cartItem.ProductId));
            if (_cartItem != null)
            {
                _cartItem.Quantity++;
                if (!cartItem.IsChecked) // Eğer işaret kaldırıldıysa
                    _cartItem.IsChecked = false;
            }
            else
            {
                await _cartItemWriteRepository.AddAsync(new()
                {
                    CartId = cart.Id,
                    ProductId = Guid.Parse(cartItem.ProductId),
                    Quantity = cartItem.Quantity,
                    IsChecked = cartItem.IsChecked // Varsayılan olarak true
                });
            }
        
            await _cartItemWriteRepository.SaveAsync();
        }
    }

    public async Task UpdateQuantityAsync(VM_Update_CartItem cartItem)
    {
        CartItem? _cartItem = await _cartItemReadRepository.GetByIdAsync(cartItem.CartItemId);
        if (_cartItem != null)
        {
            _cartItem.Quantity = cartItem.Quantity;
            await _cartItemWriteRepository.SaveAsync();
        }
    }

    public async Task RemoveCartItemAsync(string cartItemId)
    {
        CartItem? cartItem = await _cartItemReadRepository.GetByIdAsync(cartItemId);
        if (cartItem != null)
        {
            _cartItemWriteRepository.Remove(cartItem);
            await _cartItemWriteRepository.SaveAsync();
        }
    }
    public async Task UpdateCartItemAsync(VM_Update_CartItem cartItem)
    {
        CartItem? _cartItem = await _cartItemReadRepository.GetByIdAsync(cartItem.CartItemId);
        if (_cartItem != null)
        {
            _cartItem.IsChecked = cartItem.IsChecked; // Checkbox durumu da güncelleniyor
            await _cartItemWriteRepository.SaveAsync();
        }
    }

    public async Task<Cart?> GetUserActiveCart()
    {
        Cart? cart = await ContextUser();
        return await _cartReadRepository.Table
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(c => c.Id == cart.Id);
    }
}