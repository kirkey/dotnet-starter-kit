namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Add.v1;

public class AddPurchaseOrderItemCommandValidator : AbstractValidator<AddPurchaseOrderItemCommand>
{
    public AddPurchaseOrderItemCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty().WithMessage("PurchaseOrderId is required");
        RuleFor(x => x.GroceryItemId).NotEmpty().WithMessage("GroceryItemId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(100000).WithMessage("Quantity is unrealistically large");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price must be non-negative");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).When(x => x.Discount.HasValue)
            .WithMessage("Discount must be non-negative");

        // Ensure discount does not exceed line total (checked in handler as well)
        RuleFor(x => x).Must(cmd =>
        {
            if (!cmd.Discount.HasValue) return true;
            var max = cmd.Quantity * cmd.UnitPrice;
            return cmd.Discount.Value <= max;
        }).WithMessage("Discount cannot exceed line total (quantity * unit price)");
    }
}

