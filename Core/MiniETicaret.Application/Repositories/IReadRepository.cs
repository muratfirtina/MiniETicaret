using System.Linq.Expressions;
using MiniETicaret.Domain.Entities.Common;

namespace MiniETicaret.Application.Repositories;

public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll(bool tracking = true);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetByIdAsync(string? id, bool tracking = true);
    Task<bool> ExistAsync(Expression<Func<T, bool>> method, bool tracking = true);
    Task<bool> ExistByIdAsync(string? id,bool tracking = true);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> method, bool tracking = true);

}