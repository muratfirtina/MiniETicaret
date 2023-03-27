using MiniETicaret.Application.Abstractions;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Concretes;

public class ProductService : IProductService
{
    public List<Product> GetProducts()
        => new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Stock = 10,
                Price = 1000,
                CreatedDate = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Stock = 20,
                Price = 2000,
                CreatedDate = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 3",
                Stock = 30,
                Price = 3000,
                CreatedDate = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 4",
                Stock = 40,
                Price = 4000,
                CreatedDate = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 5",
                Stock = 50,
                Price = 5000,
                CreatedDate = DateTime.Now
            }
        };
}