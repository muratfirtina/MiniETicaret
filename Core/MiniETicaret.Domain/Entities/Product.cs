using MiniETicaret.Domain.Entities.Common;

namespace MiniETicaret.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    
    //public ICollection<Order> Orders { get; set; }
    public ICollection<ProductImageFile> ProductImageFiles { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    
}
