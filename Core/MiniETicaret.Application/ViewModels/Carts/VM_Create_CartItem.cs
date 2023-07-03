namespace MiniETicaret.Application.ViewModels.Carts;

public class VM_Create_CartItem
{
    public string CartId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public bool IsChecked { get; set; } = true;


}