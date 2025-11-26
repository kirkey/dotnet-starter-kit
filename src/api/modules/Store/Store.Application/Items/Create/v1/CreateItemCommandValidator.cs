using FSH.Starter.WebApi.Store.Application.Items.Specs;

namespace FSH.Starter.WebApi.Store.Application.Items.Create.v1;

/// <summary>
/// Validator for CreateItemCommand with comprehensive validation rules.
/// Validates field lengths using domain constants, uniqueness checks for SKU and Barcode, and enforces business rules.
/// </summary>
public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator(
        [FromKeyedServices("store:items")] IReadRepository<Item> repository)
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(Item.NameMinLength)
            .MaximumLength(Item.NameMaxLength);

        RuleFor(c => c.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(Item.SkuMaxLength)
            .MustAsync(async (sku, ct) =>
            {
                if (string.IsNullOrWhiteSpace(sku)) return true;
                var existingItem = await repository.FirstOrDefaultAsync(
                    new ItemBySkuSpec(sku.Trim()), ct).ConfigureAwait(false);
                return existingItem is null;
            })
            .WithMessage("Item with this SKU already exists.");

        RuleFor(c => c.Barcode)
            .NotEmpty().WithMessage("Barcode is required.")
            .MaximumLength(Item.BarcodeMaxLength)
            .MustAsync(async (barcode, ct) =>
            {
                if (string.IsNullOrWhiteSpace(barcode)) return true;
                var existingItem = await repository.FirstOrDefaultAsync(
                    new ItemByBarcodeSpec(barcode.Trim()), ct).ConfigureAwait(false);
                return existingItem is null;
            })
            .WithMessage("Item with this Barcode already exists.");

        RuleFor(c => c.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be zero or greater.")
            .GreaterThanOrEqualTo(c => c.Cost).WithMessage("UnitPrice must be greater than or equal to Cost.");

        RuleFor(c => c.Cost)
            .GreaterThanOrEqualTo(0).WithMessage("Cost must be zero or greater.");

        RuleFor(c => c.MinimumStock)
            .GreaterThanOrEqualTo(0).WithMessage("MinimumStock must be zero or greater.")
            .LessThanOrEqualTo(c => c.MaximumStock).WithMessage("MinimumStock cannot be greater than MaximumStock.");

        RuleFor(c => c.MaximumStock)
            .GreaterThan(0).WithMessage("MaximumStock must be greater than zero.");

        RuleFor(c => c.ReorderPoint)
            .GreaterThanOrEqualTo(0).WithMessage("ReorderPoint must be zero or greater.")
            .LessThanOrEqualTo(c => c.MaximumStock).WithMessage("ReorderPoint cannot exceed MaximumStock.");

        RuleFor(c => c.ReorderQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("ReorderQuantity must be zero or greater.");

        RuleFor(c => c.LeadTimeDays)
            .GreaterThanOrEqualTo(0).WithMessage("LeadTimeDays must be zero or greater.");

        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(c => c.SupplierId)
            .NotEmpty().WithMessage("SupplierId is required.");

        RuleFor(c => c.UnitOfMeasure)
            .NotEmpty().WithMessage("UnitOfMeasure is required.")
            .MaximumLength(Item.UnitOfMeasureMaxLength);

        RuleFor(c => c.ShelfLifeDays)
            .GreaterThan(0).When(c => c.IsPerishable && c.ShelfLifeDays.HasValue)
            .WithMessage("ShelfLifeDays must be greater than zero for perishable items.");

        RuleFor(c => c.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Weight must be zero or greater.");

        RuleFor(c => c.WeightUnit)
            .NotEmpty().When(c => c.Weight > 0).WithMessage("WeightUnit is required when Weight > 0.")
            .MaximumLength(Item.WeightUnitMaxLength).When(c => !string.IsNullOrWhiteSpace(c.WeightUnit));

        RuleFor(c => c.Notes)
            .MaximumLength(Item.NotesMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.Notes));

        RuleFor(c => c.Length)
            .GreaterThanOrEqualTo(0).When(c => c.Length.HasValue)
            .WithMessage("Length must be zero or greater.");

        RuleFor(c => c.Width)
            .GreaterThanOrEqualTo(0).When(c => c.Width.HasValue)
            .WithMessage("Width must be zero or greater.");

        RuleFor(c => c.Height)
            .GreaterThanOrEqualTo(0).When(c => c.Height.HasValue)
            .WithMessage("Height must be zero or greater.");

        RuleFor(c => c.DimensionUnit)
            .NotEmpty().When(c => c.Length.HasValue || c.Width.HasValue || c.Height.HasValue)
            .WithMessage("DimensionUnit is required when dimensions are specified.")
            .MaximumLength(Item.DimensionUnitMaxLength).When(c => !string.IsNullOrWhiteSpace(c.DimensionUnit));

        RuleFor(c => c.Brand)
            .MaximumLength(Item.BrandMaxLength).When(c => !string.IsNullOrWhiteSpace(c.Brand));

        RuleFor(c => c.Manufacturer)
            .MaximumLength(Item.ManufacturerMaxLength).When(c => !string.IsNullOrWhiteSpace(c.Manufacturer));

        RuleFor(c => c.ManufacturerPartNumber)
            .MaximumLength(Item.ManufacturerPartNumberMaxLength).When(c => !string.IsNullOrWhiteSpace(c.ManufacturerPartNumber));
    }
}
