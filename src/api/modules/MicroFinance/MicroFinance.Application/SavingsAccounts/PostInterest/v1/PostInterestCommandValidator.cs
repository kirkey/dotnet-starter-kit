using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.PostInterest.v1;

/// <summary>
/// Validator for PostInterestCommand.
/// </summary>
public class PostInterestCommandValidator : AbstractValidator<PostInterestCommand>
{
    public PostInterestCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required.");

        RuleFor(x => x.InterestAmount)
            .GreaterThan(0).WithMessage("Interest amount must be greater than zero.");
    }
}
