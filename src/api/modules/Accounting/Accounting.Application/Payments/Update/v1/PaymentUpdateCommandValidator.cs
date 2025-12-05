namespace Accounting.Application.Payments.Update.v1;

/// <summary>
/// Validator for UpdatePaymentCommand.
/// </summary>
public sealed class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePaymentCommandValidator"/> class.
    /// </summary>
    public UpdatePaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Payment ID is required.");

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

