using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Application.PaymentAllocations.Validators;

/// <summary>
/// Validator for UpdatePaymentAllocationCommand.
/// Ensures update requirements are met with strict validation.
/// </summary>
public class UpdatePaymentAllocationCommandValidator : AbstractValidator<UpdatePaymentAllocationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePaymentAllocationCommandValidator"/> class.
    /// </summary>
    public UpdatePaymentAllocationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Payment allocation ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Allocation amount must be greater than zero.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Allocation amount must not exceed 999,999,999.99.")
            .When(x => x.Amount.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x)
            .Must(x => x.Amount.HasValue || x.Notes != null)
            .WithMessage("At least one field (Amount or Notes) must be provided for update.");
    }
}

