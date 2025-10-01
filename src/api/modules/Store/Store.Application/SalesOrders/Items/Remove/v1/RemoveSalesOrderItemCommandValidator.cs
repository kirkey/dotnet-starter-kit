namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Remove.v1;

public class RemoveSalesOrderItemCommandValidator : AbstractValidator<RemoveSalesOrderItemCommand>
{
    public RemoveSalesOrderItemCommandValidator()
    {
        RuleFor(x => x.SalesOrderId).NotEmpty().WithMessage("SalesOrderId is required");
        RuleFor(x => x.GroceryItemId).NotEmpty().WithMessage("GroceryItemId is required");
    }
}

