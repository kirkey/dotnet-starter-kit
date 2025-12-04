using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PostInterest.v1;

/// <summary>
/// Validator for PostFixedDepositInterestCommand.
/// </summary>
public class PostFixedDepositInterestCommandValidator : AbstractValidator<PostFixedDepositInterestCommand>
{
    public PostFixedDepositInterestCommandValidator()
    {
        RuleFor(x => x.DepositId)
            .NotEmpty().WithMessage("Deposit ID is required.");

        RuleFor(x => x.InterestAmount)
            .GreaterThan(0).WithMessage("Interest amount must be greater than zero.");
    }
}
