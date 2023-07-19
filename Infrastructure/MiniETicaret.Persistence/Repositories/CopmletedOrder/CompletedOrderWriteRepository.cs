using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories.CopmletedOrder;

public class CompletedOrderWriteRepository: WriteRepository<CompletedOrder>, ICompletedOrderWriteRepository
{
    public CompletedOrderWriteRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}