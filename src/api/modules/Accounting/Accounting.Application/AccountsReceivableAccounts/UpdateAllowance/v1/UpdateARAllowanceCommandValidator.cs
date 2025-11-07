namespace Accounting.Application.AccountsReceivableAccounts.UpdateAllowance.v1;

public sealed class UpdateARAllowanceCommandValidator : AbstractValidator<UpdateARAllowanceCommand>
{
    public UpdateARAllowanceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AR account ID is required.");
        RuleFor(x => x.AllowanceAmount).GreaterThanOrEqualTo(0).WithMessage("Allowance amount must be non-negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Allowance must not exceed 999,999,999.99.");
    }
}

