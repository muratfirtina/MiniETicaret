using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class CartWriteRepository: WriteRepository<Cart>, ICartWriteRepository
{
    public CartWriteRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}