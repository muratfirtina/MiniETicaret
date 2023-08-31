using MediatR;

namespace MiniETicaret.Application.Features.Queries.Category.GetAllCategories;

public class GetAllCategoriesQueryRequest: IRequest<GetAllCategoriesQueryResponse>
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}