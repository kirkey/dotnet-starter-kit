namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;

public class CreateGroceryItemCommandValidator : AbstractValidator<CreateGroceryItemCommand>
{
    public CreateGroceryItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.SKU)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("SKU must contain only uppercase letters and numbers");

        RuleFor(x => x.Barcode)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Barcode must contain only uppercase letters and numbers");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThan(1000000);

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0)
            .LessThan(1000000);

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MaximumStock)
            .GreaterThan(0)
            .GreaterThanOrEqualTo(x => x.MinimumStock);

        RuleFor(x => x.CurrentStock)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ReorderPoint)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.MaximumStock);

        RuleFor(x => x.Weight)
            .GreaterThan(0);

        RuleFor(x => x.WeightUnit)
            .MaximumLength(20);

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.SupplierId)
            .NotEmpty();

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.IsPerishable && x.ExpiryDate.HasValue);

        RuleFor(x => x.Brand)
            .MaximumLength(100);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(100);
    }
}
