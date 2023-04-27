using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Repositories;
using MiniETicaret.Application.RequestParameters;
using MiniETicaret.Application.ViewModels.Products;
using MiniETicaret.Domain.Entities;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        
        public ProductsController(
            IProductWriteRepository productWriteRepository, 
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)
                .OrderBy(p => p.Id)
                .Skip(pagination.Page * pagination.Size)
                .Take(pagination.Size)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.CreatedDate,
                    p.UpdatedDate
                });
            return Ok(new
            {
                totalCount,
                products
            });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            if (ModelState.IsValid)
            {
                
            }
            await _productWriteRepository.AddAsync(new Product
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok(new
            {
                message="Silme işlemi başarılı"
            });
        }
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload()
        {
            
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resources", "product-images");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string noExtension = Path.GetFileNameWithoutExtension(file.FileName)
                        .Replace(" ", "-").Replace("ğ", "g").Replace("ı", "i").Replace("ö", "o")
                        .Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("Ç", "c")
                        .Replace("Ş", "s").Replace("Ğ", "g").Replace("Ü", "u").Replace("İ", "i")
                        .Replace("Ö", "o");
                    
                    Guid guid = Guid.NewGuid();
                    string fullPath = Path.Combine(uploadPath, $"{noExtension + "-"}{guid}{Path.GetExtension(file.FileName)}");
                    using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync:false))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            return Ok();
        }
    }
}
