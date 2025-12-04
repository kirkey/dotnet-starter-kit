using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Update.v1;

public sealed class UpdateLoanProductCommandValidator : AbstractValidator<UpdateLoanProductCommand>
{
    public UpdateLoanProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Loan product ID is required.");

        RuleFor(x => x.Name)
            .MinimumLength(LoanProduct.NameMinLength)
            .MaximumLength(LoanProduct.NameMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage($"Name must be between {LoanProduct.NameMinLength} and {LoanProduct.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(LoanProduct.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {LoanProduct.DescriptionMaxLength} characters.");

        RuleFor(x => x.InterestRate)
            .InclusiveBetween(LoanProduct.MinInterestRate, LoanProduct.MaxInterestRate)
            .When(x => x.InterestRate.HasValue)
            .WithMessage($"Interest rate must be between {LoanProduct.MinInterestRate} and {LoanProduct.MaxInterestRate}.");

        RuleFor(x => x.InterestMethod)
            .MaximumLength(LoanProduct.InterestMethodMaxLength)
            .When(x => !string.IsNullOrEmpty(x.InterestMethod))
            .WithMessage("Interest method must be valid.");

        RuleFor(x => x.MinLoanAmount)
            .GreaterThan(0)
            .When(x => x.MinLoanAmount.HasValue)
            .WithMessage("Minimum loan amount must be greater than 0.");

        RuleFor(x => x.MaxLoanAmount)
            .GreaterThan(x => x.MinLoanAmount ?? 0)
            .When(x => x.MaxLoanAmount.HasValue && x.MinLoanAmount.HasValue)
            .WithMessage("Maximum loan amount must be greater than minimum loan amount.");

        RuleFor(x => x.MinTermMonths)
            .GreaterThan(0)
            .When(x => x.MinTermMonths.HasValue)
            .WithMessage("Minimum term months must be greater than 0.");

        RuleFor(x => x.MaxTermMonths)
            .GreaterThanOrEqualTo(x => x.MinTermMonths ?? 0)
            .When(x => x.MaxTermMonths.HasValue && x.MinTermMonths.HasValue)
            .WithMessage("Maximum term months must be greater than or equal to minimum term months.");

        RuleFor(x => x.RepaymentFrequency)
            .MaximumLength(LoanProduct.RepaymentFrequencyMaxLength)
            .When(x => !string.IsNullOrEmpty(x.RepaymentFrequency))
            .WithMessage("Repayment frequency must be valid.");

        RuleFor(x => x.GracePeriodDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.GracePeriodDays.HasValue)
            .WithMessage("Grace period days must be 0 or greater.");

        RuleFor(x => x.LatePenaltyRate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .When(x => x.LatePenaltyRate.HasValue)
            .WithMessage("Late penalty rate must be between 0 and 100.");
    }
}
