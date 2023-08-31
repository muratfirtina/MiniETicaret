using MiniETicaret.Application.DTOs.Category;
using MiniETicaret.Application.ViewModels.Category;

namespace MiniETicaret.Application.Abstractions.Services;

public interface ICategoryService
{
    Task<CreateCategoryDto>CreateCategoryAsync(VM_Create_Category vmCreateCategory);
    Task<UpdateCategoryDto>UpdateCategoryAsync(VM_Update_Category vmUpdateCategory);
    Task<ListCategoryDto>GetAllCategoriesAsync(int page, int size);
    Task<SingleCategoryDto>GetCategoryByIdAsync(string id);
    Task<bool> DeleteCategoryAsync(string id);
    Task<CategoryDto> GetCategoryIdByNameAsync(string name);
    
    //Task<CreateSubCategoryDto>CreateSubCategoryAsync(VM_CreateSubCategory vmCreateSubCategory);
    
}