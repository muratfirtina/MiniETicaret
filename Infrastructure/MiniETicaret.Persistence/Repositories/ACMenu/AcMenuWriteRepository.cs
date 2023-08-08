using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class AcMenuWriteRepository: WriteRepository<ACMenu>, IAcMenuWriteRepository
{
    public AcMenuWriteRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}