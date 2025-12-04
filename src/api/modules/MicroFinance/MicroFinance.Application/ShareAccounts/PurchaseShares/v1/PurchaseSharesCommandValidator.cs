using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PurchaseShares.v1;

public sealed class PurchaseSharesCommandValidator : AbstractValidator<PurchaseSharesCommand>
{
    public PurchaseSharesCommandValidator()
    {
        RuleFor(x => x.ShareAccountId)
            .NotEmpty()
            .WithMessage("Share account ID is required.");

        RuleFor(x => x.NumberOfShares)
            .GreaterThan(0)
            .WithMessage("Number of shares must be greater than 0.");

        RuleFor(x => x.PricePerShare)
            .GreaterThan(0)
            .WithMessage("Price per share must be greater than 0.");
    }
}
