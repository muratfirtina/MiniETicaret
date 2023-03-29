using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities.Common;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence.Repositories;

public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    private readonly MiniETicaretDbContext _context;

    public ReadRepository(MiniETicaretDbContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();
    public IQueryable<T> GetAll()
        => Table;

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
        => Table.Where(method);

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method)
        => await Table.FirstOrDefaultAsync(method);

    public Task<T> GetByIdAsync(string id)
        => Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
}