namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Ship.v1;

public class ShipSalesOrderItemCommandValidator : AbstractValidator<ShipSalesOrderItemCommand>
{
    public ShipSalesOrderItemCommandValidator()
    {
        RuleFor(x => x.SalesOrderItemId).NotEmpty().WithMessage("SalesOrderItemId is required");

        RuleFor(x => x.ShippedQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Shipped quantity cannot be negative")
            .LessThanOrEqualTo(100000).WithMessage("Shipped quantity is unrealistically large");
    }
}

