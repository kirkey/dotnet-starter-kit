namespace Accounting.Application.Invoices.Commands;

/// <summary>
/// Validator for creating an invoice.
/// </summary>
public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceNumber)
            .NotEmpty()
            .WithMessage("Invoice number is required")
            .MaximumLength(64)
            .WithMessage("Invoice number cannot exceed 50 characters");

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required");

        RuleFor(x => x.InvoiceDate)
            .NotEmpty()
            .WithMessage("Invoice date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Invoice date cannot be in the future");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required")
            .GreaterThanOrEqualTo(x => x.InvoiceDate)
            .WithMessage("Due date must be on or after invoice date");

        RuleFor(x => x.UsageCharge)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Usage charge cannot be negative");

        RuleFor(x => x.BasicServiceCharge)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Basic service charge cannot be negative");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tax amount cannot be negative");

        RuleFor(x => x.OtherCharges)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Other charges cannot be negative");

        RuleFor(x => x.KWhUsed)
            .GreaterThanOrEqualTo(0)
            .WithMessage("kWh used cannot be negative");

        RuleFor(x => x.BillingPeriod)
            .NotEmpty()
            .WithMessage("Billing period is required")
            .MaximumLength(64)
            .WithMessage("Billing period cannot exceed 50 characters");

        When(x => x.LateFee.HasValue, () =>
        {
            RuleFor(x => x.LateFee!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Late fee cannot be negative");
        });

        When(x => x.ReconnectionFee.HasValue, () =>
        {
            RuleFor(x => x.ReconnectionFee!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Reconnection fee cannot be negative");
        });

        When(x => x.DepositAmount.HasValue, () =>
        {
            RuleFor(x => x.DepositAmount!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Deposit amount cannot be negative");
        });

        When(x => x.DemandCharge.HasValue, () =>
        {
            RuleFor(x => x.DemandCharge!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Demand charge cannot be negative");
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

