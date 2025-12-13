namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Create.v1;

/// <summary>
/// Validator for CreateItemSupplierCommand.
/// </summary>
public sealed class CreateItemSupplierCommandValidator : AbstractValidator<CreateItemSupplierCommand>
{
    public CreateItemSupplierCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId is required");

        RuleFor(x => x.SupplierId)
            .NotEmpty()
            .WithMessage("SupplierId is required");

        RuleFor(x => x.UnitCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("UnitCost cannot be negative");

        RuleFor(x => x.LeadTimeDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("LeadTimeDays cannot be negative");

        RuleFor(x => x.MinimumOrderQuantity)
            .GreaterThan(0)
            .WithMessage("MinimumOrderQuantity must be positive");

        RuleFor(x => x.SupplierPartNumber)
            .MaximumLength(128)
            .WithMessage("SupplierPartNumber must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SupplierPartNumber));

        RuleFor(x => x.PackagingQuantity)
            .GreaterThan(0)
            .WithMessage("PackagingQuantity must be positive")
            .When(x => x.PackagingQuantity.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
