using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Approve.v1;

/// <summary>
/// Validator for the ApproveFeeWaiverCommand.
/// </summary>
public class ApproveFeeWaiverCommandValidator : AbstractValidator<ApproveFeeWaiverCommand>
{
    public ApproveFeeWaiverCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Fee waiver ID is required.");

        RuleFor(x => x.ApprovedByUserId)
            .NotEmpty()
            .WithMessage("Approver user ID is required.");

        RuleFor(x => x.ApproverName)
            .NotEmpty()
            .MaximumLength(FeeWaiver.ApprovedByMaxLength)
            .WithMessage($"Approver name is required and must not exceed {FeeWaiver.ApprovedByMaxLength} characters.");
    }
}
