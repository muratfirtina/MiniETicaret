using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Queries.Category.GetAllCategories;

public class GetAllCategoriesQueryHandler: IRequestHandler<GetAllCategoriesQueryRequest, GetAllCategoriesQueryResponse>
{
    readonly ICategoryService _categoryService;

    public GetAllCategoriesQueryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<GetAllCategoriesQueryResponse> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
    {
        var data = await _categoryService.GetAllCategoriesAsync(request.Page, request.Size);
        return new()
        {
            TotalCategoryCount = data.TotalCategoryCount,
            Categories = data.Categories
        };
        
    }
}