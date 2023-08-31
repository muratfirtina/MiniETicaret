namespace MiniETicaret.Application.ViewModels.Category;

public class VM_Create_Category
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ParentCategoryName { get; set; } = null;
}