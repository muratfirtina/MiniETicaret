using MediatR;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.ViewModels.Category;

namespace MiniETicaret.Application.Features.Commands.Category.CreateCategory;

public class CreateCategoryCommandHandler: IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    readonly ICategoryService _categoryService;

    public CreateCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
    {
         await _categoryService.CreateCategoryAsync(new ()
         {
             Name = request.Name,
             Description = request.Description,
             ParentCategoryName = request.ParentCategoryName
             
         });
        return new();
    }
}