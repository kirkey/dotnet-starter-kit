using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Reject.v1;

/// <summary>
/// Validator for the RejectFeeWaiverCommand.
/// </summary>
public class RejectFeeWaiverCommandValidator : AbstractValidator<RejectFeeWaiverCommand>
{
    public RejectFeeWaiverCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Fee waiver ID is required.");

        RuleFor(x => x.RejectedByUserId)
            .NotEmpty()
            .WithMessage("Rejector user ID is required.");

        RuleFor(x => x.RejectorName)
            .NotEmpty()
            .MaximumLength(FeeWaiver.ApprovedByMaxLength)
            .WithMessage($"Rejector name is required and must not exceed {FeeWaiver.ApprovedByMaxLength} characters.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(FeeWaiver.RejectionReasonMaxLength)
            .WithMessage($"Rejection reason is required and must not exceed {FeeWaiver.RejectionReasonMaxLength} characters.");
    }
}
