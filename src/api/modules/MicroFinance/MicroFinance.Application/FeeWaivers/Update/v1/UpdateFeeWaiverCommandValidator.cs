using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Update.v1;

/// <summary>
/// Validator for the UpdateFeeWaiverCommand.
/// </summary>
public class UpdateFeeWaiverCommandValidator : AbstractValidator<UpdateFeeWaiverCommand>
{
    public UpdateFeeWaiverCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Fee waiver ID is required.");

        RuleFor(x => x.WaivedAmount)
            .GreaterThan(0)
            .When(x => x.WaivedAmount.HasValue);

        RuleFor(x => x.WaiverReason)
            .MaximumLength(FeeWaiver.WaiverReasonMaxLength)
            .When(x => !string.IsNullOrEmpty(x.WaiverReason));

        RuleFor(x => x.Notes)
            .MaximumLength(FeeWaiver.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
