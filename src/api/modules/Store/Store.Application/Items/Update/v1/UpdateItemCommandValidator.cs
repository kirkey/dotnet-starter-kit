namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

/// <summary>
/// Validator for UpdateItemCommand.
/// Applies comprehensive validation rules for all item properties.
/// </summary>
public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters.");

        RuleFor(c => c.Description)
            .MaximumLength(1000).When(c => !string.IsNullOrWhiteSpace(c.Description))
            .WithMessage("Description must not exceed 1000 characters.");

        RuleFor(c => c.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.")
            .MinimumLength(1).WithMessage("SKU must be at least 1 character.")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("SKU must contain only uppercase letters, numbers, hyphens, and underscores.");

        RuleFor(c => c.Barcode)
            .NotEmpty().WithMessage("Barcode is required.")
            .MaximumLength(100).WithMessage("Barcode must not exceed 100 characters.")
            .MinimumLength(1).WithMessage("Barcode must be at least 1 character.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Barcode must contain only uppercase letters and numbers.");

        RuleFor(c => c.UnitPrice)
            .NotNull().WithMessage("Unit Price is required.")
            .GreaterThanOrEqualTo(0).When(c => c.UnitPrice.HasValue)
            .WithMessage("Unit Price must be zero or greater.")
            .LessThanOrEqualTo(1000000).When(c => c.UnitPrice.HasValue)
            .WithMessage("Unit Price must not exceed 1,000,000.");

        RuleFor(c => c.Cost)
            .NotNull().WithMessage("Cost is required.")
            .GreaterThanOrEqualTo(0).When(c => c.Cost.HasValue)
            .WithMessage("Cost must be zero or greater.")
            .LessThanOrEqualTo(1000000).When(c => c.Cost.HasValue)
            .WithMessage("Cost must not exceed 1,000,000.");

        RuleFor(c => c.MinimumStock)
            .GreaterThanOrEqualTo(0).When(c => c.MinimumStock.HasValue)
            .WithMessage("Minimum Stock must be zero or greater.")
            .LessThanOrEqualTo(1000000).When(c => c.MinimumStock.HasValue)
            .WithMessage("Minimum Stock must not exceed 1,000,000.");

        RuleFor(c => c.MaximumStock)
            .GreaterThanOrEqualTo(0).When(c => c.MaximumStock.HasValue)
            .WithMessage("Maximum Stock must be zero or greater.")
            .LessThanOrEqualTo(10000000).When(c => c.MaximumStock.HasValue)
            .WithMessage("Maximum Stock must not exceed 10,000,000.")
            .GreaterThanOrEqualTo(c => c.MinimumStock ?? 0).When(c => c.MaximumStock.HasValue && c.MinimumStock.HasValue)
            .WithMessage("Maximum Stock must be greater than or equal to Minimum Stock.");

        RuleFor(c => c.ReorderPoint)
            .GreaterThanOrEqualTo(0).When(c => c.ReorderPoint.HasValue)
            .WithMessage("Reorder Point must be zero or greater.")
            .LessThanOrEqualTo(1000000).When(c => c.ReorderPoint.HasValue)
            .WithMessage("Reorder Point must not exceed 1,000,000.")
            .GreaterThanOrEqualTo(c => c.MinimumStock ?? 0).When(c => c.ReorderPoint.HasValue && c.MinimumStock.HasValue)
            .WithMessage("Reorder Point should be greater than or equal to Minimum Stock.");

        RuleFor(c => c.ReorderQuantity)
            .GreaterThanOrEqualTo(0).When(c => c.ReorderQuantity.HasValue)
            .WithMessage("Reorder Quantity must be zero or greater.")
            .LessThanOrEqualTo(1000000).When(c => c.ReorderQuantity.HasValue)
            .WithMessage("Reorder Quantity must not exceed 1,000,000.");

        RuleFor(c => c.LeadTimeDays)
            .GreaterThanOrEqualTo(0).When(c => c.LeadTimeDays.HasValue)
            .WithMessage("Lead Time Days must be zero or greater.")
            .LessThanOrEqualTo(365).When(c => c.LeadTimeDays.HasValue)
            .WithMessage("Lead Time Days must not exceed 365 days.");

        RuleFor(c => c.ShelfLifeDays)
            .GreaterThan(0).When(c => c.IsPerishable == true && c.ShelfLifeDays.HasValue)
            .WithMessage("Shelf Life Days must be greater than zero for perishable items.")
            .LessThanOrEqualTo(3650).When(c => c.ShelfLifeDays.HasValue)
            .WithMessage("Shelf Life Days must not exceed 3650 days (10 years).");

        RuleFor(c => c.ShelfLifeDays)
            .NotNull().When(c => c.IsPerishable == true)
            .WithMessage("Shelf Life Days is required for perishable items.");

        RuleFor(c => c.Brand)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Brand))
            .WithMessage("Brand must not exceed 200 characters.");

        RuleFor(c => c.Manufacturer)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Manufacturer))
            .WithMessage("Manufacturer must not exceed 200 characters.");

        RuleFor(c => c.ManufacturerPartNumber)
            .MaximumLength(100).When(c => !string.IsNullOrWhiteSpace(c.ManufacturerPartNumber))
            .WithMessage("Manufacturer Part Number must not exceed 100 characters.");

        RuleFor(c => c.Weight)
            .GreaterThanOrEqualTo(0).When(c => c.Weight.HasValue)
            .WithMessage("Weight must be zero or greater.")
            .LessThanOrEqualTo(100000).When(c => c.Weight.HasValue)
            .WithMessage("Weight must not exceed 100,000.");

        RuleFor(c => c.WeightUnit)
            .MaximumLength(10).When(c => !string.IsNullOrWhiteSpace(c.WeightUnit))
            .WithMessage("Weight Unit must not exceed 10 characters.");

        RuleFor(c => c.Length)
            .GreaterThanOrEqualTo(0).When(c => c.Length.HasValue)
            .WithMessage("Length must be zero or greater.")
            .LessThanOrEqualTo(100000).When(c => c.Length.HasValue)
            .WithMessage("Length must not exceed 100,000.");

        RuleFor(c => c.Width)
            .GreaterThanOrEqualTo(0).When(c => c.Width.HasValue)
            .WithMessage("Width must be zero or greater.")
            .LessThanOrEqualTo(100000).When(c => c.Width.HasValue)
            .WithMessage("Width must not exceed 100,000.");

        RuleFor(c => c.Height)
            .GreaterThanOrEqualTo(0).When(c => c.Height.HasValue)
            .WithMessage("Height must be zero or greater.")
            .LessThanOrEqualTo(100000).When(c => c.Height.HasValue)
            .WithMessage("Height must not exceed 100,000.");

        RuleFor(c => c.DimensionUnit)
            .MaximumLength(10).When(c => !string.IsNullOrWhiteSpace(c.DimensionUnit))
            .WithMessage("Dimension Unit must not exceed 10 characters.");

        RuleFor(c => c.UnitOfMeasure)
            .NotEmpty().WithMessage("Unit of Measure is required.")
            .MaximumLength(20).WithMessage("Unit of Measure must not exceed 20 characters.");

        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(c => c.SupplierId)
            .NotEmpty().WithMessage("Supplier is required.");
    }
}
