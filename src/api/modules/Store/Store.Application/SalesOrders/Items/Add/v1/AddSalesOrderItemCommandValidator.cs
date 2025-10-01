namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Add.v1;

public class AddSalesOrderItemCommandValidator : AbstractValidator<AddSalesOrderItemCommand>
{
    public AddSalesOrderItemCommandValidator()
    {
        RuleFor(x => x.SalesOrderId)
            .NotEmpty()
            .WithMessage("SalesOrderId is required");

        RuleFor(x => x.GroceryItemId)
            .NotEmpty()
            .WithMessage("GroceryItemId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(100000).WithMessage("Quantity is unrealistically large");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0m).WithMessage("Unit price must be non-negative")
            .LessThanOrEqualTo(1_000_000m).WithMessage("Unit price is unrealistically large");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0m).When(x => x.Discount.HasValue)
            .WithMessage("Discount must be non-negative");

        // Ensure discount does not exceed line total (quantity * unit price)
        RuleFor(x => x).Must(cmd =>
        {
            if (!cmd.Discount.HasValue) return true;
            // defensive: if quantity or unitprice are invalid (<=0), skip this check - other rules will fail earlier
            if (cmd.Quantity <= 0 || cmd.UnitPrice < 0m) return true;
            var max = cmd.Quantity * cmd.UnitPrice;
            return cmd.Discount.Value <= max;
        }).WithMessage("Discount cannot exceed line total (quantity * unit price)");
    }
}

