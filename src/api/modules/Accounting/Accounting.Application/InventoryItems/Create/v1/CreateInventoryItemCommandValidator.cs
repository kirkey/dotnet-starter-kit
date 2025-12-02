namespace Accounting.Application.InventoryItems.Create.v1;

public sealed class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
    {
        RuleFor(x => x.Sku).NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(64).WithMessage("SKU must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("SKU can only contain letters, numbers, hyphens, and underscores.");
        
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 200 characters.");
        
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Quantity must not exceed 999,999,999.99.");
        
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Unit price must not exceed 999,999,999.99.");
        
        RuleFor(x => x.Description).MaximumLength(1024).WithMessage("Description must not exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

