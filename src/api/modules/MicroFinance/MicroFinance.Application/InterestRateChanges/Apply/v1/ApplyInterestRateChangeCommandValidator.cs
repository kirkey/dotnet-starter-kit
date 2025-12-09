using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Apply.v1;

/// <summary>
/// Validator for the ApplyInterestRateChangeCommand.
/// </summary>
public class ApplyInterestRateChangeCommandValidator : AbstractValidator<ApplyInterestRateChangeCommand>
{
    public ApplyInterestRateChangeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Interest rate change ID is required.");
    }
}
