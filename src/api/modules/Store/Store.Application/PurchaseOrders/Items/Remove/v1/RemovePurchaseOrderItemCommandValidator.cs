namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Remove.v1;

public class RemovePurchaseOrderItemCommandValidator : AbstractValidator<RemovePurchaseOrderItemCommand>
{
    public RemovePurchaseOrderItemCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
    }
}

