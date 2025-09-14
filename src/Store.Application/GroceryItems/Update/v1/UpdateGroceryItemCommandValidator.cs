namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Update.v1;

public class UpdateGroceryItemCommandValidator : AbstractValidator<UpdateGroceryItemCommand>
{
    public UpdateGroceryItemCommandValidator([FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> readRepository)
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
            .GreaterThanOrEqualTo(0)
            .LessThan(1000000)
            .When(x => x.Price != 0);

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0)
            .LessThan(1000000)
            .When(x => x.Cost != 0);

        // If both provided, price >= cost
        RuleFor(x => x)
            .Must(x => x.Price == 0 || x.Cost == 0 || x.Price >= x.Cost)
            .WithMessage("Price must be greater than or equal to Cost when both are provided");

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinimumStock != 0);

        RuleFor(x => x.MaximumStock)
            .GreaterThan(0)
            .When(x => x.MaximumStock != 0)
            .Must((cmd, max) => cmd.MinimumStock == 0 || max >= cmd.MinimumStock)
            .WithMessage("MaximumStock must be greater than or equal to MinimumStock");

        RuleFor(x => x.ReorderPoint)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ReorderPoint != 0);

        RuleFor(x => x.CurrentStock)
            .GreaterThanOrEqualTo(0)
            .When(x => x.CurrentStock != 0)
            .WithMessage("CurrentStock must be non-negative");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0)
            .LessThan(100000)
            .When(x => x.Weight != 0);

        RuleFor(x => x.WeightUnit)
            .MaximumLength(20)
            .When(x => x.Weight != 0)
            .WithMessage("WeightUnit is required when Weight > 0");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.IsPerishable && x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future for perishable items");

        RuleFor(x => x.Brand)
            .MaximumLength(100)
            .When(x => x.Brand != null);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(100)
            .When(x => x.Manufacturer != null);

        // Async uniqueness checks that exclude the current entity by Id
        RuleFor(x => x.SKU).MustAsync(async (cmd, sku, ct) =>
        {
            if (string.IsNullOrWhiteSpace(sku)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemBySkuSpec(sku), ct).ConfigureAwait(false);
            return existing is null || existing.Id == cmd.Id;
        }).WithMessage("Another grocery item with the same SKU already exists.");

        RuleFor(x => x.Barcode).MustAsync(async (cmd, barcode, ct) =>
        {
            if (string.IsNullOrWhiteSpace(barcode)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.GroceryItemByBarcodeSpec(barcode), ct).ConfigureAwait(false);
            return existing is null || existing.Id == cmd.Id;
        }).WithMessage("Another grocery item with the same Barcode already exists.");
    }
}
