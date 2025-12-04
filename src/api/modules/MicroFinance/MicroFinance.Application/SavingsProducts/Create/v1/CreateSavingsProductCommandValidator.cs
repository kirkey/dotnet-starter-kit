using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Create.v1;

public sealed class CreateSavingsProductCommandValidator : AbstractValidator<CreateSavingsProductCommand>
{
    public CreateSavingsProductCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(SavingsProduct.CodeMaxLength)
            .WithMessage($"Code must not exceed {SavingsProduct.CodeMaxLength} characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(SavingsProduct.NameMinLength)
            .MaximumLength(SavingsProduct.NameMaxLength)
            .WithMessage($"Name must be between {SavingsProduct.NameMinLength} and {SavingsProduct.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(SavingsProduct.DescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description must not exceed {SavingsProduct.DescriptionMaxLength} characters.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .MaximumLength(SavingsProduct.CurrencyCodeMaxLength)
            .WithMessage($"Currency code must not exceed {SavingsProduct.CurrencyCodeMaxLength} characters.");

        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Interest rate must be between 0 and 100.");

        RuleFor(x => x.InterestCalculation)
            .NotEmpty()
            .MaximumLength(SavingsProduct.InterestCalculationMaxLength)
            .WithMessage($"Interest calculation must not exceed {SavingsProduct.InterestCalculationMaxLength} characters.");

        RuleFor(x => x.InterestPostingFrequency)
            .NotEmpty()
            .MaximumLength(SavingsProduct.InterestPostingFrequencyMaxLength)
            .WithMessage($"Interest posting frequency must not exceed {SavingsProduct.InterestPostingFrequencyMaxLength} characters.");

        RuleFor(x => x.MinOpeningBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum opening balance must be 0 or greater.");

        RuleFor(x => x.MinBalanceForInterest)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum balance for interest must be 0 or greater.");

        RuleFor(x => x.MinWithdrawalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum withdrawal amount must be 0 or greater.");

        RuleFor(x => x.MaxWithdrawalPerDay)
            .GreaterThan(0)
            .When(x => x.MaxWithdrawalPerDay.HasValue)
            .WithMessage("Maximum withdrawal per day must be greater than 0.");

        RuleFor(x => x.OverdraftLimit)
            .GreaterThan(0)
            .When(x => x.AllowOverdraft && x.OverdraftLimit.HasValue)
            .WithMessage("Overdraft limit must be greater than 0 when overdraft is allowed.");
    }
}
