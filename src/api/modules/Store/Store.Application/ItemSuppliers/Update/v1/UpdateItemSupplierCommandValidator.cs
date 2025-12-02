namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;

/// <summary>
/// Validator for UpdateItemSupplierCommand.
/// </summary>
public sealed class UpdateItemSupplierCommandValidator : AbstractValidator<UpdateItemSupplierCommand>
{
    public UpdateItemSupplierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        // New: ensure the UI-sent keys are present
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("ItemId is required");

        RuleFor(x => x.SupplierId)
            .NotEmpty()
            .WithMessage("SupplierId is required");

        RuleFor(x => x.UnitCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("UnitCost cannot be negative")
            .When(x => x.UnitCost.HasValue);

        RuleFor(x => x.LeadTimeDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("LeadTimeDays cannot be negative")
            .When(x => x.LeadTimeDays.HasValue);

        RuleFor(x => x.MinimumOrderQuantity)
            .GreaterThan(0)
            .WithMessage("MinimumOrderQuantity must be positive")
            .When(x => x.MinimumOrderQuantity.HasValue);

        RuleFor(x => x.SupplierPartNumber)
            .MaximumLength(128)
            .WithMessage("SupplierPartNumber must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SupplierPartNumber));

        RuleFor(x => x.PackagingQuantity)
            .GreaterThan(0)
            .WithMessage("PackagingQuantity must be positive")
            .When(x => x.PackagingQuantity.HasValue);

        RuleFor(x => x.ReliabilityRating)
            .InclusiveBetween(0, 100)
            .WithMessage("ReliabilityRating must be between 0 and 100")
            .When(x => x.ReliabilityRating.HasValue);

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
