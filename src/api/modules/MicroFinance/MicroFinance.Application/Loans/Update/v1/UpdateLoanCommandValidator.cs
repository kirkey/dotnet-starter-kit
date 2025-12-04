using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Update.v1;

public sealed class UpdateLoanCommandValidator : AbstractValidator<UpdateLoanCommand>
{
    public UpdateLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Loan ID is required.");

        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.InterestRate.HasValue)
            .WithMessage("Interest rate must be non-negative.");

        RuleFor(x => x.InterestRate)
            .LessThanOrEqualTo(100)
            .When(x => x.InterestRate.HasValue)
            .WithMessage("Interest rate cannot exceed 100%.");

        RuleFor(x => x.TermMonths)
            .GreaterThan(0)
            .When(x => x.TermMonths.HasValue)
            .WithMessage("Term months must be greater than 0.");

        RuleFor(x => x.Purpose)
            .MaximumLength(LoanConstants.PurposeMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Purpose))
            .WithMessage($"Purpose must not exceed {LoanConstants.PurposeMaxLength} characters.");

        RuleFor(x => x.RepaymentFrequency)
            .MaximumLength(LoanConstants.RepaymentFrequencyMaxLength)
            .Must(BeValidRepaymentFrequency!)
            .When(x => !string.IsNullOrWhiteSpace(x.RepaymentFrequency))
            .WithMessage("Repayment frequency must be 'Daily', 'Weekly', 'Biweekly', or 'Monthly'.");
    }

    private static bool BeValidRepaymentFrequency(string frequency) =>
        frequency is "Daily" or "Weekly" or "Biweekly" or "Monthly";
}
