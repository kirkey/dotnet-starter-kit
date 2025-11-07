namespace Accounting.Application.Payments.Delete.v1;

/// <summary>
/// Validator for PaymentDeleteCommand.
/// </summary>
public sealed class PaymentDeleteCommandValidator : AbstractValidator<PaymentDeleteCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentDeleteCommandValidator"/> class.
    /// </summary>
    public PaymentDeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Payment ID is required.");
    }
}

