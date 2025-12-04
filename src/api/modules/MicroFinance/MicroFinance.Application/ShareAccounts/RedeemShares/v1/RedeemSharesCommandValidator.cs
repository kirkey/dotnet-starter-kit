using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.RedeemShares.v1;

/// <summary>
/// Validator for RedeemSharesCommand.
/// </summary>
public class RedeemSharesCommandValidator : AbstractValidator<RedeemSharesCommand>
{
    public RedeemSharesCommandValidator()
    {
        RuleFor(x => x.ShareAccountId)
            .NotEmpty().WithMessage("Share account ID is required.");

        RuleFor(x => x.NumberOfShares)
            .GreaterThan(0).WithMessage("Number of shares must be greater than zero.");

        RuleFor(x => x.PricePerShare)
            .GreaterThan(0).WithMessage("Price per share must be greater than zero.");
    }
}
