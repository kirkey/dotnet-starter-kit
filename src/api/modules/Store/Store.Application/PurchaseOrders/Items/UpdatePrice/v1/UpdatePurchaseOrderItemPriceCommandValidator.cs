// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/Store.Application/PurchaseOrders/Items/UpdatePrice/v1/UpdatePurchaseOrderItemPriceCommandValidator.cs

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdatePrice.v1;

public class UpdatePurchaseOrderItemPriceCommandValidator : AbstractValidator<UpdatePurchaseOrderItemPriceCommand>
{
    public UpdatePurchaseOrderItemPriceCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderItemId)
            .NotEmpty()
            .WithMessage("PurchaseOrderItemId is required");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0m).WithMessage("Unit price must be non-negative")
            .LessThanOrEqualTo(1_000_000m).WithMessage("Unit price is unrealistically large");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0m).When(x => x.DiscountAmount.HasValue)
            .WithMessage("Discount must be non-negative");

        // Note: a validation that discount doesn't exceed line total requires the current item quantity,
        // which isn't available in this command validator. The aggregate/handler will enforce that rule
        // (and throw a domain exception) to avoid querying state from the validator.
    }
}

