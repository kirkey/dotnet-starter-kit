using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Cancel.v1;

/// <summary>
/// Validator for the CancelInterestRateChangeCommand.
/// </summary>
public class CancelInterestRateChangeCommandValidator : AbstractValidator<CancelInterestRateChangeCommand>
{
    public CancelInterestRateChangeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Interest rate change ID is required.");
    }
}
