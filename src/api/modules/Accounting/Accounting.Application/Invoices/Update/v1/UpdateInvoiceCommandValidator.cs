namespace Accounting.Application.Invoices.Update.v1;

/// <summary>
/// Validator for invoice update command.
/// </summary>
public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty()
            .WithMessage("Invoice ID is required");

        When(x => x.UsageCharge.HasValue, () =>
        {
            RuleFor(x => x.UsageCharge!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Usage charge cannot be negative");
        });

        When(x => x.BasicServiceCharge.HasValue, () =>
        {
            RuleFor(x => x.BasicServiceCharge!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Basic service charge cannot be negative");
        });

        When(x => x.TaxAmount.HasValue, () =>
        {
            RuleFor(x => x.TaxAmount!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tax amount cannot be negative");
        });

        When(x => x.OtherCharges.HasValue, () =>
        {
            RuleFor(x => x.OtherCharges!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Other charges cannot be negative");
        });

        When(x => !string.IsNullOrWhiteSpace(x.RateSchedule), () =>
        {
            RuleFor(x => x.RateSchedule!)
                .MaximumLength(128)
                .WithMessage("Rate schedule cannot exceed 100 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(512)
                .WithMessage("Description cannot exceed 500 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Notes), () =>
        {
            RuleFor(x => x.Notes!)
                .MaximumLength(1024)
                .WithMessage("Notes cannot exceed 1000 characters");
        });
    }
}

