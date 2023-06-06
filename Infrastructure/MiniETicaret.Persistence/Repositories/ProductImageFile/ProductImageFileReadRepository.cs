using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
{
    public ProductImageFileReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}
