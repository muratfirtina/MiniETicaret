namespace MiniETicaret.Application.DTOs.Product;

public class ProductDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
}