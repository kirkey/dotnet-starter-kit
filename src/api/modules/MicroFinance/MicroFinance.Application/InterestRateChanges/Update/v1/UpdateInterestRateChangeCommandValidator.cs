using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Update.v1;

/// <summary>
/// Validator for the UpdateInterestRateChangeCommand.
/// </summary>
public class UpdateInterestRateChangeCommandValidator : AbstractValidator<UpdateInterestRateChangeCommand>
{
    public UpdateInterestRateChangeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Interest rate change ID is required.");

        RuleFor(x => x.ChangeType)
            .MaximumLength(InterestRateChange.ChangeTypeMaxLength)
            .When(x => !string.IsNullOrEmpty(x.ChangeType));

        RuleFor(x => x.NewRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.NewRate.HasValue);

        RuleFor(x => x.ChangeReason)
            .MaximumLength(InterestRateChange.ChangeReasonMaxLength)
            .When(x => !string.IsNullOrEmpty(x.ChangeReason));

        RuleFor(x => x.Notes)
            .MaximumLength(InterestRateChange.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
