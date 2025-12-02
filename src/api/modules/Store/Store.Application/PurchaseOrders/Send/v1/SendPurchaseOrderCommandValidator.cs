namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Send.v1;

/// <summary>
/// Validator for SendPurchaseOrderCommand.
/// Ensures the purchase order ID is valid for sending to supplier.
/// </summary>
public sealed class SendPurchaseOrderCommandValidator : AbstractValidator<SendPurchaseOrderCommand>
{
    public SendPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Purchase order ID is required");

        RuleFor(x => x.DeliveryInstructions)
            .MaximumLength(1024)
            .WithMessage("Delivery instructions must not exceed 1000 characters");
    }
}
