using Microsoft.EntityFrameworkCore;
using MiniETicaret.Domain.Entities.Common;

namespace MiniETicaret.Application.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    DbSet<T> Table { get; }
}