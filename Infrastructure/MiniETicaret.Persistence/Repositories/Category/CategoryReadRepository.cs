using MiniETicaret.Application.Repositories;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories.Category;

public class CategoryReadRepository : ReadRepository<Domain.Entities.Category>, ICategoryReadRepository
{
    public CategoryReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}