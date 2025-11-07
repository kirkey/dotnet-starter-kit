namespace Accounting.Application.Payments.Update.v1;

/// <summary>
/// Validator for PaymentUpdateCommand.
/// </summary>
public sealed class PaymentUpdateCommandValidator : AbstractValidator<PaymentUpdateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentUpdateCommandValidator"/> class.
    /// </summary>
    public PaymentUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Payment ID is required.");

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(100)
            .WithMessage("Reference number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.DepositToAccountCode)
            .MaximumLength(50)
            .WithMessage("Deposit account code must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.DepositToAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .WithMessage("Notes must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

