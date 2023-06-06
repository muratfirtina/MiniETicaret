using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class CartReadRepository: ReadRepository<Cart>, ICartReadRepository
{
    public CartReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}