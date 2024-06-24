
using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Name is required")
                .MinimumLength(2)
                .MaximumLength(150)
                    .WithMessage("Name is required and must be between 2 and 150 characters");

            RuleFor(x => x.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Price is required")
                .Must(s => s >= 0)
                    .WithMessage("Price must be greater than or equal to 0");

            RuleFor(x => x.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stock is required")
                .Must(s => s >= 0)
                    .WithMessage("Price must be greater than or equal to 0");
        }
    }
}
