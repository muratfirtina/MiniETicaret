using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence.Repositories;

public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
{
    public CustomerReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}