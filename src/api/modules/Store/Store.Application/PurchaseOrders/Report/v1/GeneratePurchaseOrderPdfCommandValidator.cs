namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1;

/// <summary>
/// Validator for the GeneratePurchaseOrderPdfCommand.
/// </summary>
public sealed class GeneratePurchaseOrderPdfCommandValidator : AbstractValidator<GeneratePurchaseOrderPdfCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneratePurchaseOrderPdfCommandValidator"/> class.
    /// </summary>
    public GeneratePurchaseOrderPdfCommandValidator()
    {
        RuleFor(x => x.PurchaseOrderId)
            .NotEmpty()
            .WithMessage("Purchase Order ID is required.");
    }
}

