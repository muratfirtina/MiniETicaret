using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Concretes;

namespace MiniETicaret.Persistence.Repositories;

public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
{
    public CustomerWriteRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}