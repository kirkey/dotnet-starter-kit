namespace FSH.Starter.WebApi.Store.Application.Items.Create.v1;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(c => c.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.");

        RuleFor(c => c.Barcode)
            .NotEmpty().WithMessage("Barcode is required.")
            .MaximumLength(100).WithMessage("Barcode must not exceed 100 characters.");

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
            .MaximumLength(20).WithMessage("UnitOfMeasure must not exceed 20 characters.");

        RuleFor(c => c.ShelfLifeDays)
            .GreaterThan(0).When(c => c.IsPerishable && c.ShelfLifeDays.HasValue)
            .WithMessage("ShelfLifeDays must be greater than zero for perishable items.");

        RuleFor(c => c.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Weight must be zero or greater.");

        RuleFor(c => c.WeightUnit)
            .NotEmpty().When(c => c.Weight > 0).WithMessage("WeightUnit is required when Weight > 0.")
            .MaximumLength(20).WithMessage("WeightUnit must not exceed 20 characters.");

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
            .MaximumLength(20).WithMessage("DimensionUnit must not exceed 20 characters.");

        RuleFor(c => c.Brand)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Brand))
            .WithMessage("Brand must not exceed 200 characters.");

        RuleFor(c => c.Manufacturer)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Manufacturer))
            .WithMessage("Manufacturer must not exceed 200 characters.");

        RuleFor(c => c.ManufacturerPartNumber)
            .MaximumLength(100).When(c => !string.IsNullOrWhiteSpace(c.ManufacturerPartNumber))
            .WithMessage("ManufacturerPartNumber must not exceed 100 characters.");
    }
}
