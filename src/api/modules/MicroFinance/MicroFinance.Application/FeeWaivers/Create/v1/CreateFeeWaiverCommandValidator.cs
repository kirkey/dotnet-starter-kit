using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Create.v1;

/// <summary>
/// Validator for the CreateFeeWaiverCommand.
/// </summary>
public class CreateFeeWaiverCommandValidator : AbstractValidator<CreateFeeWaiverCommand>
{
    public CreateFeeWaiverCommandValidator()
    {
        RuleFor(x => x.FeeChargeId)
            .NotEmpty()
            .WithMessage("Fee charge ID is required.");

        RuleFor(x => x.Reference)
            .NotEmpty()
            .MaximumLength(FeeWaiver.ReferenceMaxLength)
            .WithMessage($"Reference is required and must not exceed {FeeWaiver.ReferenceMaxLength} characters.");

        RuleFor(x => x.OriginalAmount)
            .GreaterThan(0)
            .WithMessage("Original amount must be greater than zero.");

        RuleFor(x => x.WaivedAmount)
            .GreaterThan(0)
            .LessThanOrEqualTo(x => x.OriginalAmount)
            .WithMessage("Waived amount must be greater than zero and not exceed original amount.");

        RuleFor(x => x.WaiverReason)
            .NotEmpty()
            .MaximumLength(FeeWaiver.WaiverReasonMaxLength)
            .WithMessage($"Waiver reason is required and must not exceed {FeeWaiver.WaiverReasonMaxLength} characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(FeeWaiver.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
