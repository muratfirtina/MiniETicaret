using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Category;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.ViewModels.Category;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Services;

public class CategoryService:ICategoryService
{
    readonly ICategoryReadRepository _categoryReadRepository;
    readonly ICategoryWriteRepository _categoryWriteRepository;

    public CategoryService(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _categoryWriteRepository = categoryWriteRepository;
    }

    public async Task<CreateCategoryDto> CreateCategoryAsync(VM_Create_Category vmCreateCategory)
    {
        // Kontrol edilecek adımdan önce var olan bir kategori var mı diye kontrol edin.
        var categoryExists = await _categoryReadRepository.ExistAsync(c => c.Name == vmCreateCategory.Name);

        if (categoryExists)
        {
            throw new Exception("Bu isimde bir kategori zaten var.");
        }

        // ParentCategoryId ve ParentCategoryName değerlerini varsayılan olarak ayarlayın.
        Guid? parentCategoryId = null;
        string parentCategoryName = null;

        if (!string.IsNullOrEmpty(vmCreateCategory.ParentCategoryName))
        {
            // Eğer ParentCategoryName değeri girilmişse, ilgili üst kategoriyi veritabanından bulun.
            var parentCategory = await _categoryReadRepository.FirstOrDefaultAsync(c => c.Name == vmCreateCategory.ParentCategoryName);

            if (parentCategory == null)
            {
                throw new Exception("Belirtilen üst kategori bulunamadı.");
            }

            parentCategoryId = parentCategory.Id;
            parentCategoryName = parentCategory.Name;
        }

        // Yeni kategori oluşturulacak veriyi hazırlayın.
        var createCategoryDto = new CreateCategoryDto
        {
            CategoryId = Guid.NewGuid().ToString(),
            Name = vmCreateCategory.Name,
            Description = vmCreateCategory.Description,
            ParentCategoryId = parentCategoryId?.ToString(),
            ParentCategoryName = parentCategoryName
        };

        var newCategory = new Category
        {
            Id = Guid.Parse(createCategoryDto.CategoryId),
            Name = createCategoryDto.Name,
            Description = createCategoryDto.Description,
            ParentCategoryId = parentCategoryId
        };

        // Veritabanına yeni kategoriyi ekleyin.
        await _categoryWriteRepository.AddAsync(newCategory);
    
        // Tüm işlemleri tamamlayıp veritabanını kaydedin.
        await _categoryWriteRepository.SaveAsync();

        return createCategoryDto;
    }


    public Task<UpdateCategoryDto> UpdateCategoryAsync(VM_Update_Category vmUpdateCategory)
    {
        throw new NotImplementedException();
    }

    public async Task<ListCategoryDto> GetAllCategoriesAsync(int page, int size)
    {
        var query = _categoryReadRepository.GetAll(false);
        
        IQueryable<Category>? categoriesQuery;
        if (page == -1 && size == -1)
        {
            categoriesQuery = query;
        }
        else
        {
            categoriesQuery = query.Skip(page * size).Take(size);
        }
        
        var categories = await categoriesQuery
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.CreatedDate,
                c.UpdatedDate
            }).ToListAsync();
        
        var totalCategoryCount = page >= 0 && size >= 0
            ? await _categoryReadRepository.GetAll(false).CountAsync()
            : categories.Count;
        
        return new ListCategoryDto
        {
            TotalCategoryCount = totalCategoryCount,
            Categories = categories
        };
        
    }

    public Task<SingleCategoryDto> GetCategoryByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCategoryAsync(string id)
    {
        await _categoryWriteRepository.RemoveAsync(id);
        await _categoryWriteRepository.SaveAsync();
        return true;
        
    }
    
    //kategori name den id bul.
    public async Task<CategoryDto> GetCategoryIdByNameAsync(string name)
    {
        var category = await _categoryReadRepository.FirstOrDefaultAsync(c => c.Name == name);
        return new CategoryDto
        {
            Id = category.Id.ToString(),
            Name = category.Name
        };
    }
    
    
}