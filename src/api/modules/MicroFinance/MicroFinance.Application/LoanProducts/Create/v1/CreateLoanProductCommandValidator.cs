using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Create.v1;

public sealed class CreateLoanProductCommandValidator : AbstractValidator<CreateLoanProductCommand>
{
    public CreateLoanProductCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(LoanProduct.CodeMaxLength)
            .WithMessage($"Code must not exceed {LoanProduct.CodeMaxLength} characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(LoanProduct.NameMinLength)
            .MaximumLength(LoanProduct.NameMaxLength)
            .WithMessage($"Name must be between {LoanProduct.NameMinLength} and {LoanProduct.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(LoanProduct.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {LoanProduct.DescriptionMaxLength} characters.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(LoanProduct.CurrencyCodeMaxLength)
            .WithMessage($"Currency code must not exceed {LoanProduct.CurrencyCodeMaxLength} characters.");

        RuleFor(x => x.InterestRate)
            .InclusiveBetween(LoanProduct.MinInterestRate, LoanProduct.MaxInterestRate)
            .WithMessage($"Interest rate must be between {LoanProduct.MinInterestRate} and {LoanProduct.MaxInterestRate}.");

        RuleFor(x => x.InterestMethod)
            .NotEmpty()
            .MaximumLength(LoanProduct.InterestMethodMaxLength)
            .WithMessage("Interest method is required.");

        RuleFor(x => x.MinLoanAmount)
            .GreaterThan(0)
            .WithMessage("Minimum loan amount must be greater than 0.");

        RuleFor(x => x.MaxLoanAmount)
            .GreaterThan(x => x.MinLoanAmount)
            .WithMessage("Maximum loan amount must be greater than minimum loan amount.");

        RuleFor(x => x.MinTermMonths)
            .GreaterThan(0)
            .WithMessage("Minimum term months must be greater than 0.");

        RuleFor(x => x.MaxTermMonths)
            .GreaterThanOrEqualTo(x => x.MinTermMonths)
            .WithMessage("Maximum term months must be greater than or equal to minimum term months.");

        RuleFor(x => x.RepaymentFrequency)
            .NotEmpty()
            .MaximumLength(LoanProduct.RepaymentFrequencyMaxLength)
            .WithMessage("Repayment frequency is required.");

        RuleFor(x => x.GracePeriodDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Grace period days must be 0 or greater.");

        RuleFor(x => x.LatePenaltyRate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Late penalty rate must be between 0 and 100.");
    }
}
