using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Application.PaymentAllocations.Validators;

/// <summary>
/// Validator for CreatePaymentAllocationCommand.
/// Ensures all payment allocation requirements are met with strict validation.
/// </summary>
public class CreatePaymentAllocationCommandValidator : AbstractValidator<CreatePaymentAllocationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePaymentAllocationCommandValidator"/> class.
    /// </summary>
    public CreatePaymentAllocationCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("Payment ID is required.");

        RuleFor(x => x.InvoiceId)
            .NotEmpty()
            .WithMessage("Invoice ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Allocation amount must be greater than zero.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Allocation amount must not exceed 999,999,999.99.");

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

