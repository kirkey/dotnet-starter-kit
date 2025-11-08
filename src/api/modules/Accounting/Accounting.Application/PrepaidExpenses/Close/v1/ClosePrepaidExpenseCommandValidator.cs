namespace Accounting.Application.PrepaidExpenses.Close.v1;

public sealed class ClosePrepaidExpenseCommandValidator : AbstractValidator<ClosePrepaidExpenseCommand>
{
    public ClosePrepaidExpenseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Prepaid expense ID is required.");
    }
}

