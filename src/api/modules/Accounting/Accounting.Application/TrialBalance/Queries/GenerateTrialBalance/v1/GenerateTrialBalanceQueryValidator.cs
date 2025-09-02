using FluentValidation;

namespace Accounting.Application.TrialBalance.Queries.GenerateTrialBalance.v1;

public class GenerateTrialBalanceQueryValidator : AbstractValidator<GenerateTrialBalanceQuery>
{
    public GenerateTrialBalanceQueryValidator()
    {
        RuleFor(x => x.AsOfDate)
            .NotEmpty()
            .WithMessage("As of date is required")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("As of date cannot be in the future");

        RuleFor(x => x.AccountTypeFilter)
            .MaximumLength(50)
            .WithMessage("Account type filter cannot exceed 50 characters");
    }
}
