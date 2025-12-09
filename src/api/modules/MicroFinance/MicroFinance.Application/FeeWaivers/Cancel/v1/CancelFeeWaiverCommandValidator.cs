using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Cancel.v1;

/// <summary>
/// Validator for the CancelFeeWaiverCommand.
/// </summary>
public class CancelFeeWaiverCommandValidator : AbstractValidator<CancelFeeWaiverCommand>
{
    public CancelFeeWaiverCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Fee waiver ID is required.");
    }
}
