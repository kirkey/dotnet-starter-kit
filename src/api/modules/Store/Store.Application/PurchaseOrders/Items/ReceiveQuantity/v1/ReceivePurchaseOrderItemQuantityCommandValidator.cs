namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.ReceiveQuantity.v1;

public class ReceivePurchaseOrderItemQuantityCommandValidator : AbstractValidator<ReceivePurchaseOrderItemQuantityCommand>
{
    public ReceivePurchaseOrderItemQuantityCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderItemId).NotEmpty().WithMessage("PurchaseOrderItemId is required");

        RuleFor(x => x.ReceivedQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Received quantity cannot be negative")
            .LessThanOrEqualTo(100000).WithMessage("Received quantity is unrealistically large");
    }
}

