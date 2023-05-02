using MiniETicaret.Application.Repositories;
using MiniETicaret.Persistence.Concretes;
using File = MiniETicaret.Domain.Entities.File;

namespace MiniETicaret.Persistence.Repositories;

public class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
{
    public FileWriteRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}