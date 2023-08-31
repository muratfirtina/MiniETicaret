using MediatR;
using MiniETicaret.Application.Abstractions.Services;

namespace MiniETicaret.Application.Features.Commands.Category.RemoveCategory;

public class RemoveCategoryCommandHandler: IRequestHandler<RemoveCategoryCommandRequest, RemoveCategoryCommandResponse>
{
    readonly ICategoryService _categoryService;

    public RemoveCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<RemoveCategoryCommandResponse> Handle(RemoveCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.DeleteCategoryAsync(request.Id);
        return new()
        {
            IsSuccess = result
        };
    }
}