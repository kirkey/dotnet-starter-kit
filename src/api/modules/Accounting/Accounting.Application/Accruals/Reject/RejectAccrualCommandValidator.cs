namespace Accounting.Application.Accruals.Reject;

/// <summary>
/// Validator for RejectAccrualCommand.
/// </summary>
public sealed class RejectAccrualCommandValidator : AbstractValidator<RejectAccrualCommand>
{
    public RejectAccrualCommandValidator()
    {
        RuleFor(x => x.AccrualId)
            .NotEmpty()
            .WithMessage("Accrual ID is required.");

        RuleFor(x => x.RejectedBy)
            .NotEmpty()
            .WithMessage("Rejector is required.")
            .MaximumLength(200)
            .WithMessage("Rejector name cannot exceed 200 characters.");

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}

