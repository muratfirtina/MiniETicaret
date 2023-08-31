namespace MiniETicaret.Application.DTOs.Category;

public class CreateSubCategoryDto
{
    public string SubCategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ParentCategoryId { get; set; }
    public string ParentCategoryName { get; set; }= "Parent Category";
    
    
}