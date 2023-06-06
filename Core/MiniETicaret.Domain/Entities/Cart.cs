using MiniETicaret.Domain.Entities.Common;
using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Domain.Entities;

public class Cart:BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public Order Order { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    
}