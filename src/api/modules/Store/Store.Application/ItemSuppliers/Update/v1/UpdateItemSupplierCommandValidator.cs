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
            .MaximumLength(100)
            .WithMessage("SupplierPartNumber must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SupplierPartNumber));

        RuleFor(x => x.PackagingQuantity)
            .GreaterThan(0)
            .WithMessage("PackagingQuantity must be positive")
            .When(x => x.PackagingQuantity.HasValue);

        RuleFor(x => x.CurrencyCode)
            .Length(3)
            .WithMessage("CurrencyCode must be 3 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.CurrencyCode));

        RuleFor(x => x.ReliabilityRating)
            .InclusiveBetween(0, 100)
            .WithMessage("ReliabilityRating must be between 0 and 100")
            .When(x => x.ReliabilityRating.HasValue);
    }
}
