using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence.Repositories;

public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
{
    public ProductReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}