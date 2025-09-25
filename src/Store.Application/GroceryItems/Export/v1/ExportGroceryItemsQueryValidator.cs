namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

/// <summary>
/// Validator for ExportGroceryItemsQuery with business rule validations.
/// Ensures export parameters are valid and within reasonable limits.
/// </summary>
public sealed class ExportGroceryItemsQueryValidator : AbstractValidator<ExportGroceryItemsQuery>
{
    public ExportGroceryItemsQueryValidator()
    {
        // Category ID validation - must be valid GUID if provided
        RuleFor(x => x.CategoryId)
            .Must(BeValidGuid)
            .When(x => x.CategoryId.HasValue)
            .WithMessage("CategoryId must be a valid identifier when provided.");

        // Supplier ID validation - must be valid GUID if provided  
        RuleFor(x => x.SupplierId)
            .Must(BeValidGuid)
            .When(x => x.SupplierId.HasValue)
            .WithMessage("SupplierId must be a valid identifier when provided.");

        // Search term validation - reasonable length limits
        RuleFor(x => x.SearchTerm)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("SearchTerm cannot exceed 100 characters.");

        RuleFor(x => x.SearchTerm)
            .MinimumLength(2)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("SearchTerm must be at least 2 characters when provided.");

        // Business rule validation - cannot combine contradictory filters
        RuleFor(x => x)
            .Must(x => !(x.OnlyLowStock && x.OnlyPerishable))
            .WithMessage("Cannot filter for both low stock and perishable items simultaneously for performance reasons.")
            .WithName("FilterCombination");
    }

    /// <summary>
    /// Validates that a GUID value is not empty when provided.
    /// </summary>
    /// <param name="guid">The GUID to validate</param>
    /// <returns>True if GUID is valid (not empty), false otherwise</returns>
    private static bool BeValidGuid(DefaultIdType? guid)
    {
        return !guid.HasValue || guid.Value != Guid.Empty;
    }
}
