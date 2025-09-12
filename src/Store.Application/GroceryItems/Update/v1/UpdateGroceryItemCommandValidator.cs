namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Update.v1;

public class UpdateGroceryItemCommandValidator : AbstractValidator<UpdateGroceryItemCommand>
{
    public UpdateGroceryItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.SKU)
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("SKU must contain only uppercase letters and numbers")
            .When(x => !string.IsNullOrEmpty(x.SKU));

        RuleFor(x => x.Barcode)
            .MaximumLength(100)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Barcode must contain only uppercase letters and numbers")
            .When(x => !string.IsNullOrEmpty(x.Barcode));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description != null);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThan(1000000)
            .When(x => x.Price != 0);

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0)
            .LessThan(1000000)
            .When(x => x.Cost != 0);

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinimumStock != 0);

        RuleFor(x => x.MaximumStock)
            .GreaterThan(0)
            .When(x => x.MaximumStock != 0);

        RuleFor(x => x.ReorderPoint)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ReorderPoint != 0);

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .When(x => x.Weight != 0);

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.IsPerishable && x.ExpiryDate.HasValue);

        RuleFor(x => x.Brand)
            .MaximumLength(100)
            .When(x => x.Brand != null);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(100)
            .When(x => x.Manufacturer != null);
    }
}
