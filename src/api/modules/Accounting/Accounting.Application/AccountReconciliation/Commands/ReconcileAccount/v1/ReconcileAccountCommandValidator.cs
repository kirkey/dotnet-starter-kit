using FluentValidation;

namespace Accounting.Application.AccountReconciliation.Commands.ReconcileAccount.v1;

public class ReconcileAccountCommandValidator : AbstractValidator<ReconcileAccountCommand>
{
    public ReconcileAccountCommandValidator()
    {
        RuleFor(x => x.ChartOfAccountId)
            .NotEmpty()
            .WithMessage("Chart of Account ID is required");

        RuleFor(x => x.ReconciliationDate)
            .NotEmpty()
            .WithMessage("Reconciliation date is required")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Reconciliation date cannot be in the future");

        RuleFor(x => x.ReconciliationReference)
            .MaximumLength(100)
            .WithMessage("Reconciliation reference cannot exceed 100 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters");

        RuleForEach(x => x.ReconciliationLines)
            .SetValidator(new ReconciliationLineDtoValidator());
    }
}

public class ReconciliationLineDtoValidator : AbstractValidator<ReconciliationLineDto>
{
    public ReconciliationLineDtoValidator()
    {
        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage("Transaction date is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(200)
            .WithMessage("Description cannot exceed 200 characters");

        RuleFor(x => x.Amount)
            .NotEqual(0)
            .WithMessage("Amount cannot be zero");
    }
}
