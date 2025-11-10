namespace Accounting.Application.Meters.Update.v1;

/// <summary>
/// Validator for UpdateMeterCommand.
/// </summary>
public sealed class UpdateMeterCommandValidator : AbstractValidator<UpdateMeterCommand>
{
    public UpdateMeterCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Meter ID is required.");

        RuleFor(x => x.Location)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Location))
            .WithMessage("Location cannot exceed 500 characters.");

        RuleFor(x => x.CommunicationProtocol)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.CommunicationProtocol))
            .WithMessage("Communication protocol cannot exceed 50 characters.");
    }
}

