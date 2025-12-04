using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;

public sealed class CreateSavingsAccountCommandValidator : AbstractValidator<CreateSavingsAccountCommand>
{
    public CreateSavingsAccountCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.SavingsProductId)
            .NotEmpty()
            .WithMessage("Savings product ID is required.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(8)
            .WithMessage("Currency is required and must not exceed 8 characters.");

        RuleFor(x => x.InitialDeposit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Initial deposit must be 0 or greater.");
    }
}
