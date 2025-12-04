using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Create.v1;

public sealed class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.LoanProductId)
            .NotEmpty()
            .WithMessage("Loan product ID is required.");

        RuleFor(x => x.RequestedAmount)
            .GreaterThan(0)
            .WithMessage("Requested amount must be greater than 0.");

        RuleFor(x => x.TermMonths)
            .GreaterThan(0)
            .WithMessage("Term months must be greater than 0.");

        RuleFor(x => x.Purpose)
            .MaximumLength(LoanConstants.PurposeMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Purpose))
            .WithMessage($"Purpose must not exceed {LoanConstants.PurposeMaxLength} characters.");

        RuleFor(x => x.RepaymentFrequency)
            .NotEmpty()
            .Must(BeValidRepaymentFrequency)
            .WithMessage("Repayment frequency must be 'DAILY', 'WEEKLY', 'BIWEEKLY', or 'MONTHLY'.");
    }

    private static bool BeValidRepaymentFrequency(string frequency) =>
        frequency is "DAILY" or "WEEKLY" or "BIWEEKLY" or "MONTHLY";
}
