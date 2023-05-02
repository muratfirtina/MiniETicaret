using MiniETicaret.Application.Repositories;
using MiniETicaret.Persistence.Concretes;
using File = MiniETicaret.Domain.Entities.File;

namespace MiniETicaret.Persistence.Repositories;

public class FileReadRepository : ReadRepository<File>, IFileReadRepository
{
    public FileReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}