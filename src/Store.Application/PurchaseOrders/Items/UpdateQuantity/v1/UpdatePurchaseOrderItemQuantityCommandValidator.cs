namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdateQuantity.v1;

public class UpdatePurchaseOrderItemQuantityCommandValidator : AbstractValidator<UpdatePurchaseOrderItemQuantityCommand>
{
    public UpdatePurchaseOrderItemQuantityCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderItemId).NotEmpty().WithMessage("PurchaseOrderItemId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(100000).WithMessage("Quantity is unrealistically large");
    }
}

