namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.UpdateQuantity.v1;

public class UpdateSalesOrderItemQuantityCommandValidator : AbstractValidator<UpdateSalesOrderItemQuantityCommand>
{
    public UpdateSalesOrderItemQuantityCommandValidator()
    {
        RuleFor(x => x.SalesOrderItemId).NotEmpty().WithMessage("SalesOrderItemId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(100000).WithMessage("Quantity is unrealistically large");
    }
}

