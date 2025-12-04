using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;

public sealed class TransferFundsCommandValidator : AbstractValidator<TransferFundsCommand>
{
    public TransferFundsCommandValidator()
    {
        RuleFor(x => x.FromAccountId)
            .NotEmpty()
            .WithMessage("Source account ID is required.");

        RuleFor(x => x.ToAccountId)
            .NotEmpty()
            .WithMessage("Destination account ID is required.");

        RuleFor(x => x.ToAccountId)
            .NotEqual(x => x.FromAccountId)
            .WithMessage("Cannot transfer to the same account.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Transfer amount must be greater than 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must not exceed 500 characters.");
    }
}
