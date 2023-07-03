namespace MiniETicaret.Application.ViewModels.Carts;

public class VM_Update_CartItem
{
    public string CartItemId { get; set; }
    public int Quantity { get; set; }
    public bool IsChecked { get; set; }=true;
}