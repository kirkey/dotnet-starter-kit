namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Approve.v1;

/// <summary>
/// Validator for ApprovePurchaseOrderCommand.
/// Ensures the purchase order ID is valid for approval.
/// </summary>
public sealed class ApprovePurchaseOrderCommandValidator : AbstractValidator<ApprovePurchaseOrderCommand>
{
    public ApprovePurchaseOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Purchase order ID is required");

        RuleFor(x => x.ApprovalNotes)
            .MaximumLength(512)
            .WithMessage("Approval notes must not exceed 500 characters");
    }
}
