using FluentValidation;
using MiniETicaret.Application.ViewModels.Products;

namespace MiniETicaret.Application.Validators.Products;

public class UpdateProductValidator:AbstractValidator<VM_Update_Product>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .NotNull()
                .WithMessage("Product Id not null or empty")
            .MaximumLength(150)
            .MinimumLength(5)
                .WithMessage("Product Id min length 5, max length 150");
        
        RuleFor(p=>p.Stock)
            .NotEmpty()
            .NotNull()
            .WithMessage("Stock not null or empty")
            .Must(s=>s>=0)
            .WithMessage("Stock must be greater than or equal to 0");

        RuleFor(p=>p.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Price not null or empty")
            .Must(p=>p>=0)
            .WithMessage("Price must be greater than or equal to 0");

    }
}