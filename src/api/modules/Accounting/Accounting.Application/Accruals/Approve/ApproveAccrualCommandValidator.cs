using FluentValidation;

namespace Accounting.Application.Accruals.Approve;

/// <summary>
/// Validator for ApproveAccrualCommand.
/// </summary>
public sealed class ApproveAccrualCommandValidator : AbstractValidator<ApproveAccrualCommand>
{
    public ApproveAccrualCommandValidator()
    {
        RuleFor(x => x.AccrualId)
            .NotEmpty()
            .WithMessage("Accrual ID is required.");

        RuleFor(x => x.ApprovedBy)
            .NotEmpty()
            .WithMessage("Approver is required.")
            .MaximumLength(200)
            .WithMessage("Approver name cannot exceed 200 characters.");
    }
}

