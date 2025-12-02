namespace Accounting.Application.Invoices.ApplyPayment.v1;

/// <summary>
/// Validator for apply payment command.
/// </summary>
public class ApplyInvoicePaymentCommandValidator : AbstractValidator<ApplyInvoicePaymentCommand>
{
    public ApplyInvoicePaymentCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty()
            .WithMessage("Invoice ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Payment amount must be greater than zero");

        RuleFor(x => x.PaymentDate)
            .NotEmpty()
            .WithMessage("Payment date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Payment date cannot be in the future");

        When(x => !string.IsNullOrWhiteSpace(x.PaymentMethod), () =>
        {
            RuleFor(x => x.PaymentMethod!)
                .MaximumLength(64)
                .WithMessage("Payment method cannot exceed 50 characters");
        });
    }
}

