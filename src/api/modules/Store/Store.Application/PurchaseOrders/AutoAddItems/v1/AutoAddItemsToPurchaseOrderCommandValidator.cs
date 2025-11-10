namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.AutoAddItems.v1;

/// <summary>
/// Validator for AutoAddItemsToPurchaseOrderCommand.
/// </summary>
public class AutoAddItemsToPurchaseOrderCommandValidator : AbstractValidator<AutoAddItemsToPurchaseOrderCommand>
{
    public AutoAddItemsToPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId)
            .NotEmpty()
            .WithMessage("Purchase order ID is required");
    }
}

