namespace MiniETicaret.Application.DTOs.Order;

public class CompleteOrderDTO
{
    public string OrderCode { get; set; }
    public string OrderDescription { get; set; }
    public string OrderAddress { get; set; }
    public float OrderTotalPrice { get; set; }
    public DateTime OrderCreatedDate { get; set; }
    public string UserName { get; set; }
    public List<OrderCartItemDTO> OrderCartItems { get; set; }
    public string EMail { get; set; }

}