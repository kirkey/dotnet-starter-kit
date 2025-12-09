using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Create.v1;

/// <summary>
/// Validator for the CreateInterestRateChangeCommand.
/// </summary>
public class CreateInterestRateChangeCommandValidator : AbstractValidator<CreateInterestRateChangeCommand>
{
    public CreateInterestRateChangeCommandValidator()
    {
        RuleFor(x => x.LoanId)
            .NotEmpty()
            .WithMessage("Loan ID is required.");

        RuleFor(x => x.Reference)
            .NotEmpty()
            .MaximumLength(InterestRateChange.ReferenceMaxLength)
            .WithMessage($"Reference is required and must not exceed {InterestRateChange.ReferenceMaxLength} characters.");

        RuleFor(x => x.ChangeType)
            .NotEmpty()
            .MaximumLength(InterestRateChange.ChangeTypeMaxLength)
            .WithMessage($"Change type is required and must not exceed {InterestRateChange.ChangeTypeMaxLength} characters.");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage("Effective date is required.");

        RuleFor(x => x.PreviousRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Previous rate must be zero or greater.");

        RuleFor(x => x.NewRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("New rate must be zero or greater.");

        RuleFor(x => x.ChangeReason)
            .NotEmpty()
            .MaximumLength(InterestRateChange.ChangeReasonMaxLength)
            .WithMessage($"Change reason is required and must not exceed {InterestRateChange.ChangeReasonMaxLength} characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(InterestRateChange.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
