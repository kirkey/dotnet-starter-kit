namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;

public class CreateGroceryItemCommandValidator : AbstractValidator<CreateGroceryItemCommand>
{
    public CreateGroceryItemCommandValidator([FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> readRepository)
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
            .GreaterThanOrEqualTo(0)
            .LessThan(1000000);

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0)
            .LessThan(1000000);

        // Price should not be less than cost
        RuleFor(x => x)
            .Must(x => x.Price >= x.Cost)
            .WithMessage("Price must be greater than or equal to Cost");

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .LessThan(1000000);

        RuleFor(x => x.MaximumStock)
            .GreaterThan(0)
            .LessThan(1000000)
            .Must((cmd, max) => max >= cmd.MinimumStock)
            .WithMessage("MaximumStock must be greater than or equal to MinimumStock");

        RuleFor(x => x.CurrentStock)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.MaximumStock)
            .When(x => x.MaximumStock > 0)
            .WithMessage("CurrentStock must be between 0 and MaximumStock");

        RuleFor(x => x.ReorderPoint)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => Math.Max(x.MaximumStock, x.CurrentStock))
            .WithMessage("ReorderPoint must be between 0 and the greater of MaximumStock or CurrentStock");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0)
            .LessThan(100000);

        RuleFor(x => x.WeightUnit)
            .MaximumLength(20)
            .When(x => x.Weight > 0)
            .WithMessage("WeightUnit is required when Weight > 0");

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.SupplierId)
            .NotEmpty();

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.IsPerishable && x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future for perishable items");

        RuleFor(x => x.Brand)
            .MaximumLength(100);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(100);

        // Async uniqueness checks
        RuleFor(x => x.SKU).MustAsync(async (sku, ct) =>
        {
            if (string.IsNullOrWhiteSpace(sku)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemBySkuSpec(sku), ct).ConfigureAwait(false);
            return existing is null;
        }).WithMessage("A grocery item with the same SKU already exists.");

        RuleFor(x => x.Barcode).MustAsync(async (barcode, ct) =>
        {
            if (string.IsNullOrWhiteSpace(barcode)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemByBarcodeSpec(barcode), ct).ConfigureAwait(false);
            return existing is null;
        }).WithMessage("A grocery item with the same Barcode already exists.");
    }
}
