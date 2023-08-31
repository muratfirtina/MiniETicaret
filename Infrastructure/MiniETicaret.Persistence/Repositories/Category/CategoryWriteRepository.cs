using MiniETicaret.Application.Repositories;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories.Category;

public class CategoryWriteRepository: WriteRepository<Domain.Entities.Category>, ICategoryWriteRepository
{
    public CategoryWriteRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}