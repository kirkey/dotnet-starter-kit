namespace Accounting.Application.InventoryItems.Update.v1;

public sealed class UpdateInventoryItemCommandValidator : AbstractValidator<UpdateInventoryItemCommand>
{
    public UpdateInventoryItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory item ID is required.");
        
        RuleFor(x => x.Sku).MaximumLength(64).WithMessage("SKU must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("SKU can only contain letters, numbers, hyphens, and underscores.")
            .When(x => !string.IsNullOrWhiteSpace(x.Sku));
        
        RuleFor(x => x.Name).MaximumLength(256).WithMessage("Name must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
        
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Quantity must not exceed 999,999,999.99.")
            .When(x => x.Quantity.HasValue);
        
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Unit price must not exceed 999,999,999.99.")
            .When(x => x.UnitPrice.HasValue);
        
        RuleFor(x => x.Description).MaximumLength(1024).WithMessage("Description must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        
        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Sku) || !string.IsNullOrWhiteSpace(x.Name) || 
                                  x.Quantity.HasValue || x.UnitPrice.HasValue || x.Description != null)
            .WithMessage("At least one field must be provided for update.");
    }
}

