namespace Accounting.Application.Accruals.Approve;

/// <summary>
/// Validator for ApproveAccrualCommand.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveAccrualCommandValidator : AbstractValidator<ApproveAccrualCommand>
{
    public ApproveAccrualCommandValidator()
    {
        RuleFor(x => x.AccrualId)
            .NotEmpty()
            .WithMessage("Accrual ID is required.");
    }
}

