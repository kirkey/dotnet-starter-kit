namespace Accounting.Application.Payments.Delete.v1;

/// <summary>
/// Validator for DeletePaymentCommand.
/// </summary>
public sealed class DeletePaymentCommandValidator : AbstractValidator<DeletePaymentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePaymentCommandValidator"/> class.
    /// </summary>
    public DeletePaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Payment ID is required.");
    }
}

