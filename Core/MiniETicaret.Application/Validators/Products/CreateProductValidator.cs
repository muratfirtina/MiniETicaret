using FluentValidation;
using MiniETicaret.Application.ViewModels.Products;

namespace MiniETicaret.Application.Validators.Products;

public class CreateProductValidator : AbstractValidator<VM_Create_Product>
{
    public CreateProductValidator()
    {
        RuleFor(p=>p.Name)
            .NotEmpty()
            .NotNull()
                .WithMessage("Ürün adı boş geçilemez")
            .MaximumLength(150)
            .MinimumLength(3)
                .WithMessage("Ürün adı en az 3, en fazla 150 karakter olabilir");
        
        RuleFor(p=>p.Stock)
            .NotEmpty()
            .NotNull()
                .WithMessage("Ürün stok adedi boş geçilemez")
            .Must(s=>s>=0)
                .WithMessage("Ürün stok adedi 0 veya 0'dan büyük olmalıdır");

        RuleFor(p=>p.Price)
            .NotEmpty()
            .NotNull()
                .WithMessage("Ürün fiyatı boş geçilemez")
            .Must(p=>p>=0)
                .WithMessage("Ürün fiyatı 0 veya 0'dan büyük olmalıdır");
    }
    
}