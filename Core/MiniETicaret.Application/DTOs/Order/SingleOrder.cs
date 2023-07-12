namespace MiniETicaret.Application.DTOs.Order;

public class SingleOrder
{
    public string Address { get; set; }
    public object CartItems { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Id { get; set; }
    public string OrderCode { get; set; }
}