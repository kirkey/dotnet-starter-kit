namespace Accounting.Application.AccountReconciliations.Approve.v1;

/// <summary>
/// Validator for approve command.
/// </summary>
public sealed class ApproveAccountReconciliationCommandValidator : AbstractValidator<ApproveAccountReconciliationCommand>
{
    public ApproveAccountReconciliationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reconciliation ID is required.");

        RuleFor(x => x.ApproverId)
            .NotEmpty().WithMessage("Approver ID is required.");
    }
}

