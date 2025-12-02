namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Receive.v1;

/// <summary>
/// Validator for ReceivePurchaseOrderCommand.
/// Ensures the purchase order ID is valid and delivery date is reasonable.
/// </summary>
public sealed class ReceivePurchaseOrderCommandValidator : AbstractValidator<ReceivePurchaseOrderCommand>
{
    public ReceivePurchaseOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Purchase order ID is required");

        RuleFor(x => x.ActualDeliveryDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .When(x => x.ActualDeliveryDate.HasValue)
            .WithMessage("Actual delivery date cannot be in the future");

        RuleFor(x => x.ReceiptNotes)
            .MaximumLength(1024)
            .WithMessage("Receipt notes must not exceed 1000 characters");
    }
}
