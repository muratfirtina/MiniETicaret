using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class EndpointWriteRepository:WriteRepository<Endpoint>,IEndpointWriteRepository
{
    public EndpointWriteRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}