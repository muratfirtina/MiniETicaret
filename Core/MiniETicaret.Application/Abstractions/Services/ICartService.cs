using MiniETicaret.Application.ViewModels.Carts;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Application.Abstractions.Services;

public interface ICartService
{
    public Task<List<CartItem>> GetCartItemsAsync();
    public Task AddItemToCartAsync(VM_Create_CartItem cartItem);
    public Task UpdateQuantityAsync(VM_Update_CartItem cartItem);
    public Task RemoveCartItemAsync(string cartItemId);
    public Task UpdateCartItemAsync(VM_Update_CartItem cartItem);
    


}