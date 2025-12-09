using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Reject.v1;

/// <summary>
/// Validator for the RejectInterestRateChangeCommand.
/// </summary>
public class RejectInterestRateChangeCommandValidator : AbstractValidator<RejectInterestRateChangeCommand>
{
    public RejectInterestRateChangeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Interest rate change ID is required.");

        RuleFor(x => x.RejectedByUserId)
            .NotEmpty()
            .WithMessage("Rejector user ID is required.");

        RuleFor(x => x.RejectorName)
            .NotEmpty()
            .MaximumLength(InterestRateChange.ApprovedByMaxLength)
            .WithMessage($"Rejector name is required and must not exceed {InterestRateChange.ApprovedByMaxLength} characters.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(InterestRateChange.RejectionReasonMaxLength)
            .WithMessage($"Rejection reason is required and must not exceed {InterestRateChange.RejectionReasonMaxLength} characters.");
    }
}
