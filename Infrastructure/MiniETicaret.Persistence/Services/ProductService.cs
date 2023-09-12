using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MiniETicaret.Application.Abstractions.Services;
using MiniETicaret.Application.DTOs.Product;
using MiniETicaret.Application.DTOs.ProductImage;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.ViewModels.Products;
using MiniETicaret.Domain.Entities;

namespace MiniETicaret.Persistence.Services;

public class ProductService : IProductService
{
    readonly IProductReadRepository _productReadRepository;
    readonly IProductWriteRepository _productWriteRepository;
    readonly IQrCodeService _qrCodeService;
    readonly ICategoryService _categoryService;

    public ProductService(IProductReadRepository productReadRepository, IQrCodeService qrCodeService,
        IProductWriteRepository productWriteRepository, ICategoryService categoryService)
    {
        _productReadRepository = productReadRepository;
        _qrCodeService = qrCodeService;
        _productWriteRepository = productWriteRepository;
        _categoryService = categoryService;
    }

    public async Task<ListProductDto> GetAllProductsAsync(int page, int size)
    {
        var query = _productReadRepository.GetAll(false)
            .OrderBy(p=>p.CreatedDate);
        
        IQueryable<Product> productsQuery;

        if (page == -1 && size == -1)
        {
            productsQuery = query;
        }
        else
        {
            productsQuery = query.Skip(page * size).Take(size);
        }

        var products = await productsQuery
            .Include(p => p.ProductImageFiles)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImageFiles
            }).ToListAsync();

        var totalProductCount = page >= 0 && size >= 0
            ? await _productReadRepository.GetAll(false).CountAsync()
            : products.Count;

        return new ListProductDto
        {
            TotalProductCount = totalProductCount,
            Products = products
        };
    }

    public async Task<ProductDto> CreateProductAsync(VM_Create_Product createProductmodel)
    {
        //bu isimde bir product var mı kontrol et.
        var product = await _productReadRepository.ExistAsync(p => p.Name == createProductmodel.Name);
        if (product)
            throw new Exception("Product already exists.");
        
        var productDto = new ProductDto
        {
            Id = Guid.NewGuid().ToString(),
            Name = createProductmodel.Name,
            Price = createProductmodel.Price,
            Stock = createProductmodel.Stock,
            CategoryName = createProductmodel.CategoryName,
            CategoryId = (await _categoryService.GetCategoryIdByNameAsync(createProductmodel.CategoryName)).Id
            
            
        };
        
        var newProduct = new Product
        {
            Id = Guid.Parse(productDto.Id),
            Name = productDto.Name,
            Price = productDto.Price,
            Stock = productDto.Stock,
            CategoryId = Guid.Parse(productDto.CategoryId)
            
        };
        
        await _productWriteRepository.AddAsync(newProduct);
        await _productWriteRepository.SaveAsync();
        
        return productDto;
    }

    public async Task<ProductDto> UpdateProductAsync(VM_Update_Product updateProductModel)
    {
        
        Product product = await _productReadRepository.GetByIdAsync(updateProductModel.Id);
        if (product is null)
            throw new Exception("Product not found.");
        
        product.Name = updateProductModel.Name;
        product.Price = updateProductModel.Price;
        product.Stock = updateProductModel.Stock;
        await _productWriteRepository.SaveAsync();
        
        var productDto = new ProductDto
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };
        
        return productDto;
            
    }

    public async Task<ProductStockDto> UpdateProductOrderStockAsync(string productId, int quantity)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        if(product.Stock>0 && product.Stock >= quantity)
        {
            product.Stock -= quantity;
            await _productWriteRepository.SaveAsync();
        }
        else
        {
            throw new Exception("Stock is not enough.");
        }
        var productStockDto = new ProductStockDto
        {
            Id = product.Id.ToString(),
            Stock = product.Stock
        };
        return productStockDto;
        
    }
    
    public async Task<int> GetProductStockAsync(string productId)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        if (product is null)
            throw new Exception("Product not found.");
        return product.Stock;
    }

    public async Task<ProductDto> GetProductByIdAsync(string productId)
    {
        var product = await _productReadRepository.GetByIdAsync(productId);
        if (product is null)
            throw new Exception("Product not found.");
        return new ProductDto
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
        };
    }


    public async Task<byte[]> QrCodeToProductAsync(string productId)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        if (product is null)
            throw new Exception("Product not found.");

        var plainObject = new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.CreatedDate
        };
        string plainObjectJson = JsonSerializer.Serialize(plainObject);
        return _qrCodeService.GenerateQrCode(plainObjectJson);
    }

    public async Task StockUpdateWithQrCodeAsync(string productId, int stock)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        if (product is null)
            throw new Exception("Product not found.");

        product.Stock = stock;
        await _productWriteRepository.SaveAsync();
    }

    
}