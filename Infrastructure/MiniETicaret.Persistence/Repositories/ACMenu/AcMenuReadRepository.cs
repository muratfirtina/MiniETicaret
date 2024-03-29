using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class AcMenuReadRepository: ReadRepository<ACMenu>, IAcMenuReadRepository
{
    public AcMenuReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}
