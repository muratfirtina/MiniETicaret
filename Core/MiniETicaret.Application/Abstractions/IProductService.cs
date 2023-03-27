using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Application.Abstractions;

public interface IProductService
{
    List<Product> GetProducts();
}