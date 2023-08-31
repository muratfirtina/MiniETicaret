namespace MiniETicaret.Application.DTOs.Category;

public class CreateCategoryDto
{
    public string CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ParentCategoryId { get; set; }
    public string ParentCategoryName { get; set; }

}