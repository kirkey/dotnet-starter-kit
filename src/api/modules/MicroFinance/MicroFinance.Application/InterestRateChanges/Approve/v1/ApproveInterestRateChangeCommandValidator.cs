using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Approve.v1;

/// <summary>
/// Validator for the ApproveInterestRateChangeCommand.
/// </summary>
public class ApproveInterestRateChangeCommandValidator : AbstractValidator<ApproveInterestRateChangeCommand>
{
    public ApproveInterestRateChangeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Interest rate change ID is required.");

        RuleFor(x => x.ApprovedByUserId)
            .NotEmpty()
            .WithMessage("Approver user ID is required.");

        RuleFor(x => x.ApproverName)
            .NotEmpty()
            .MaximumLength(InterestRateChange.ApprovedByMaxLength)
            .WithMessage($"Approver name is required and must not exceed {InterestRateChange.ApprovedByMaxLength} characters.");
    }
}
