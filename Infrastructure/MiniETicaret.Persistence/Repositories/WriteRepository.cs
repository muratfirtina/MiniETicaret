using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities.Common;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence.Repositories;

public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
{
    readonly private MiniETicaretDbContext _context;

    public WriteRepository(MiniETicaretDbContext context)
    {
        _context = context;
    }
    
    public DbSet<T> Table =>_context.Set<T>();
    public async Task<bool> AddAsync(T model)
    {
      EntityEntry<T> entityEntry = await Table.AddAsync(model);
      return entityEntry.State == EntityState.Added;
    }

    public async Task<bool> AddRangeAsync(List<T> data)
    {
        await Table.AddRangeAsync(data);
        return true;
    }

    public bool Remove(T model)
    {
        EntityEntry<T> entityEntry = Table.Remove(model);
        return entityEntry.State == EntityState.Deleted;
    }

    public bool RemoveRange(List<T> data)
    {
        Table.RemoveRange(data);
        return true;
    }

    public async Task<bool> RemoveAsync(string id)
    {
       T model = await Table.FirstOrDefaultAsync( data => data.Id == Guid.Parse(id));
         return Remove(model);
    }

    public bool Update(T model)
    {
        EntityEntry<T> entityEntry = Table.Update(model);
        return entityEntry.State == EntityState.Modified;
    }

    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
}