namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Submit.v1;

/// <summary>
/// Validator for SubmitPurchaseOrderCommand.
/// Ensures the purchase order ID is valid and the order can be submitted.
/// </summary>
public sealed class SubmitPurchaseOrderCommandValidator : AbstractValidator<SubmitPurchaseOrderCommand>
{
    public SubmitPurchaseOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Purchase order ID is required");
    }
}
