namespace MiniETicaret.Application.DTOs.Category;

public class SingleCategoryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public object Products { get; set; }
    public object SubCategories { get; set; }
    
}