using MiniETicaret.Application.DTOs.ProductImage;

namespace MiniETicaret.Application.DTOs.Order;

public class OrderCartItemDTO
{
    public string Name { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public float TotalPrice { get; set; }
    public List<ProductImageFileDTO>? ProductImageFiles { get; set; }
}