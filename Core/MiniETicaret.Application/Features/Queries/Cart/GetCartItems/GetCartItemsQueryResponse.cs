namespace MiniETicaret.Application.Features.Queries.Cart.GetCartItems;

public class GetCartItemsQueryResponse
{
    public string CartItemId { get; set; }
    public string ProductName { get; set; }
    public float UnitPrice { get; set; }
    public int Quantity { get; set; }
    public List<string> ProductImageUrls { get; set; }
    
    
}