namespace Accounting.Application.Payments.Create.v1;

/// <summary>
/// Validator for CreatePaymentCommand.
/// Ensures all payment creation requirements are met.
/// </summary>
public sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePaymentCommandValidator"/> class.
    /// </summary>
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentNumber)
            .NotEmpty()
            .WithMessage("Payment number is required.")
            .MaximumLength(64)
            .WithMessage("Payment number must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("Payment number can only contain letters, numbers, and hyphens.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Payment amount must be greater than zero.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Payment amount must not exceed 999,999,999.99.");

        RuleFor(x => x.PaymentDate)
            .NotEmpty()
            .WithMessage("Payment date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Payment date cannot be in the future.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required.")
            .MaximumLength(64)
            .WithMessage("Payment method must not exceed 50 characters.")
            .Must(method => new[] { "Cash", "Check", "EFT", "CreditCard", "DebitCard", "Wire", "ACH", "Other" }
                .Contains(method, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Payment method must be one of: Cash, Check, EFT, CreditCard, DebitCard, Wire, ACH, Other.");

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(128)
            .WithMessage("Reference number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.DepositToAccountCode)
            .MaximumLength(64)
            .WithMessage("Deposit account code must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.DepositToAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

