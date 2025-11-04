namespace Accounting.Application.Invoices.Create.v1;

/// <summary>
/// Validator for creating an invoice.
/// Ensures all required fields are provided and constraints are met.
/// </summary>
public sealed class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateInvoiceCommandValidator"/> class.
    /// </summary>
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceNumber)
            .NotEmpty()
            .WithMessage("Invoice number is required.")
            .MaximumLength(50)
            .WithMessage("Invoice number cannot exceed 50 characters.")
            .Matches(@"^[A-Z0-9\-]+$")
            .WithMessage("Invoice number can only contain uppercase letters, numbers, and hyphens.");

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.InvoiceDate)
            .NotEmpty()
            .WithMessage("Invoice date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Invoice date cannot be more than one day in the future.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.")
            .GreaterThanOrEqualTo(x => x.InvoiceDate)
            .WithMessage("Due date must be on or after invoice date.")
            .LessThanOrEqualTo(x => x.InvoiceDate.AddYears(1))
            .WithMessage("Due date cannot be more than one year after invoice date.");

        RuleFor(x => x.UsageCharge)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Usage charge cannot be negative.")
            .LessThan(1000000)
            .WithMessage("Usage charge cannot exceed 999,999.");

        RuleFor(x => x.BasicServiceCharge)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Basic service charge cannot be negative.")
            .LessThan(100000)
            .WithMessage("Basic service charge cannot exceed 99,999.");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tax amount cannot be negative.")
            .LessThan(1000000)
            .WithMessage("Tax amount cannot exceed 999,999.");

        RuleFor(x => x.OtherCharges)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Other charges cannot be negative.")
            .LessThan(1000000)
            .WithMessage("Other charges cannot exceed 999,999.");

        RuleFor(x => x.KWhUsed)
            .GreaterThanOrEqualTo(0)
            .WithMessage("kWh used cannot be negative.")
            .LessThan(1000000)
            .WithMessage("kWh used cannot exceed 999,999.");

        RuleFor(x => x.BillingPeriod)
            .NotEmpty()
            .WithMessage("Billing period is required.")
            .MaximumLength(50)
            .WithMessage("Billing period cannot exceed 50 characters.");

        RuleFor(x => x.LateFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Late fee cannot be negative.")
            .LessThan(100000)
            .WithMessage("Late fee cannot exceed 99,999.")
            .When(x => x.LateFee.HasValue);

        RuleFor(x => x.ReconnectionFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Reconnection fee cannot be negative.")
            .LessThan(100000)
            .WithMessage("Reconnection fee cannot exceed 99,999.")
            .When(x => x.ReconnectionFee.HasValue);

        RuleFor(x => x.DepositAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Deposit amount cannot be negative.")
            .LessThan(1000000)
            .WithMessage("Deposit amount cannot exceed 999,999.")
            .When(x => x.DepositAmount.HasValue);

        RuleFor(x => x.DemandCharge)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Demand charge cannot be negative.")
            .LessThan(1000000)
            .WithMessage("Demand charge cannot exceed 999,999.")
            .When(x => x.DemandCharge.HasValue);

        RuleFor(x => x.RateSchedule)
            .MaximumLength(100)
            .WithMessage("Rate schedule cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.RateSchedule));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

