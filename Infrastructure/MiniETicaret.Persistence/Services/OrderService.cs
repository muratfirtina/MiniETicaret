using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Order;
using MiniETicaret.Application.DTOs.ProductImage;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.ViewModels.Products;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Services;

public class OrderService: IOrderService
{
    readonly IOrderWriteRepository _orderWriteRepository;
    readonly IOrderReadRepository _orderReadRepository;
    readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
    readonly ICompletedOrderReadRepository _completedOrderReadRepository;
    readonly ICartService _cartService;
    readonly IProductService _productService;

    public OrderService(IOrderWriteRepository orderWriteRepository, ICartService cartService, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository, ICompletedOrderReadRepository completedOrderReadRepository, IProductService productService)
    {
        _orderWriteRepository = orderWriteRepository;
        _cartService = cartService;
        _orderReadRepository = orderReadRepository;
        _completedOrderWriteRepository = completedOrderWriteRepository;
        _completedOrderReadRepository = completedOrderReadRepository;
        _productService = productService;
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        Cart? cart = await _cartService.GetUserActiveCart();
        
        // Seçili olan ürünleri siparişe bağla
        var selectedCartItems = cart?.CartItems.Where(ci => ci.IsChecked).ToList();
        if (selectedCartItems != null && selectedCartItems.Count == 0)
        {
            throw new Exception("Sipariş vermek için seçili ürün bulunamadı.");//todo: dil desteği eklenecek
        }

        // Seçili ürünlerin stok kontrolü
        foreach (var cartItem in selectedCartItems)
        {
            if (cartItem.Quantity > cartItem.Product.Stock)
            {
                throw new Exception($"{cartItem.Product.Name} ürününden {cartItem.Quantity} adet sipariş verilemez. Stokta {cartItem.Product.Stock} adet ürün bulunmaktadır.");//todo: dil desteği eklenecek
            }
            
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

        // Seçili ürünleri siparişe bağla. Bu bloğa gerek olmayabilir. sonra tekrar bakılacak
        if (selectedCartItems != null)
            foreach (var cartItem in selectedCartItems)
            {
                cartItem.IsChecked = true; // Seçili ürünlerin IsChecked değerini true yap
                newOrder.Cart?.CartItems.Add(cartItem);
            }

        // Siparişi kaydet
        await _orderWriteRepository.AddAsync(newOrder);
        await _orderWriteRepository.SaveAsync();
        
        //Siparişi verilen ürünlerin sipariş miktarı kadar stoktan düşürülmesi
        foreach (var cartItem in selectedCartItems)
        {
            await _productService.UpdateProductOrderStockAsync(cartItem.ProductId.ToString(), cartItem.Quantity);
            
        }
        
    }

    public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
    {
        var query = _orderReadRepository.Table.Include(o => o.Cart)
            .ThenInclude(c => c.User)
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product);



        var data = query.OrderBy(o=>o.CreatedDate).Skip(page * size).Take(size);
        
        var data2 = from order in data
            join completedOrder in _completedOrderReadRepository.Table
                on order.Id equals completedOrder.OrderId into co
            from _co in co.DefaultIfEmpty()
            select new
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                OrderCode = order.OrderCode,
                Cart = order.Cart,
                Completed = _co != null ? true : false
            };
        
        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data2.Select(o=> new 
            {
                Id = o.Id,
                OrderCode = o.OrderCode,
                UserName = o.Cart.User.UserName,
                TotalPrice = o.Cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                CreatedDate = o.CreatedDate,
                o.Completed
                
            }).ToListAsync()
        };

    }

    public async Task<SingleOrder> GetOrderByIdAsync(string id)
    {
        var data = _orderReadRepository.Table
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product);
            //.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));
            
            var data2 = await (from order in data
                join completedOrder in _completedOrderReadRepository.Table
                    on order.Id equals completedOrder.OrderId into co
                from _co in co.DefaultIfEmpty()
                select new
                {
                    Id = order.Id,
                    CreatedDate = order.CreatedDate,
                    OrderCode = order.OrderCode,
                    Cart = order.Cart,
                    Completed = _co != null ? true : false,
                    Address = order.Address,
                    Description = order.Description
                }).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        return new()
        {
            Id = data2.Id.ToString(),
            OrderCode = data2.OrderCode,
            Address = data2.Address,
            Description = data2.Description,
            CreatedDate = data2.CreatedDate,
            Completed = data2.Completed,
            CartItems = data2.Cart.CartItems.Select(ci => new
            {
                ci.Product.Name,
                ci.Product.Price,
                ci.Quantity,
            })
        };
        
    }

    public async Task<(bool, CompleteOrderDto)> CompleteOrderAsync(string id)
    {
        Order? order = await _orderReadRepository.Table.Include(o => o.Cart)
            .ThenInclude(c => c.User)
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.ProductImageFiles) // Include the ProductImageFiles reference
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        if (order != null)
        {
            await _completedOrderWriteRepository.AddAsync(new() { OrderId = Guid.Parse(id) });

            var orderCartItems = order.Cart.CartItems.Select(ci => new OrderCartItemDto
            {
                Name = ci.Product?.Name ?? string.Empty,
                Price = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity,
                TotalPrice = (ci.Product?.Price ?? 0) * ci.Quantity,
                ProductImageFiles = ci.Product?.ProductImageFiles
                    .Where(pif => pif.Showcase) // Filter the showcase images
                    .Select(pif => new ProductImageFileDto
                    {
                        Path = pif.Path
                    })
                    .ToList()
            }).ToList();

            float orderTotalPrice = orderCartItems.Sum(item => item.TotalPrice);

            return (await _completedOrderWriteRepository.SaveAsync() > 0, new()
            {
                OrderCode = order.OrderCode,
                OrderAddress = order.Address,
                OrderDescription = order.Description,
                UserName = order.Cart.User.UserName,
                OrderCreatedDate = order.CreatedDate,
                EMail = order.Cart.User.Email,
                OrderCartItems = orderCartItems,
                OrderTotalPrice = orderTotalPrice
            });
        }

        return (false, null);
    }



    
}