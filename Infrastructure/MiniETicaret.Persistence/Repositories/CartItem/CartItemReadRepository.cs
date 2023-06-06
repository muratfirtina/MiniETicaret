using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class CartItemReadRepository: ReadRepository<CartItem>, ICartItemReadRepository
{
    public CartItemReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}