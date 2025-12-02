namespace Accounting.Application.PrepaidExpenses.Cancel.v1;

public sealed class CancelPrepaidExpenseCommandValidator : AbstractValidator<CancelPrepaidExpenseCommand>
{
    public CancelPrepaidExpenseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Prepaid expense ID is required.");
        RuleFor(x => x.Reason).NotEmpty().WithMessage("Cancellation reason is required.")
            .MinimumLength(10).WithMessage("Reason must be at least 10 characters.")
            .MaximumLength(512).WithMessage("Reason must not exceed 500 characters.");
    }
}

