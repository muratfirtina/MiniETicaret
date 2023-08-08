using MiniETicaret.Application.Repositories;
using MiniETicaret.Domain.Entities;
using MiniETicaret.Persistence.Contexts;

namespace MiniETicaret.Persistence.Repositories;

public class EndpointReadRepository: ReadRepository<Endpoint>, IEndpointReadRepository
{
    public EndpointReadRepository(MiniETicaretDbContext context) : base(context)
    {
    }
}