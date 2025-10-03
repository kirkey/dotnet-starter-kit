namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Cancel.v1;

/// <summary>
/// Validator for CancelPurchaseOrderCommand.
/// Ensures the purchase order ID is valid for cancellation.
/// </summary>
public sealed class CancelPurchaseOrderCommandValidator : AbstractValidator<CancelPurchaseOrderCommand>
{
    public CancelPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Purchase order ID is required");

        RuleFor(x => x.CancellationReason)
            .MaximumLength(500)
            .WithMessage("Cancellation reason must not exceed 500 characters");
    }
}
