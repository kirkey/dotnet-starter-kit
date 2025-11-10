namespace Accounting.Application.Meters.Create.v1;

/// <summary>
/// Validator for CreateMeterCommand.
/// </summary>
public sealed class CreateMeterCommandValidator : AbstractValidator<CreateMeterCommand>
{
    public CreateMeterCommandValidator()
    {
        RuleFor(x => x.MeterNumber)
            .NotEmpty()
            .WithMessage("Meter number is required.")
            .MaximumLength(50)
            .WithMessage("Meter number cannot exceed 50 characters.");

        RuleFor(x => x.MeterType)
            .NotEmpty()
            .WithMessage("Meter type is required.")
            .Must(type => new[] { "Single Phase", "Three Phase", "Smart Meter", "Analog", "Digital" }
                .Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid meter type. Must be: Single Phase, Three Phase, Smart Meter, Analog, or Digital.");

        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .WithMessage("Manufacturer is required.")
            .MaximumLength(100)
            .WithMessage("Manufacturer cannot exceed 100 characters.");

        RuleFor(x => x.ModelNumber)
            .NotEmpty()
            .WithMessage("Model number is required.")
            .MaximumLength(100)
            .WithMessage("Model number cannot exceed 100 characters.");

        RuleFor(x => x.InstallationDate)
            .NotEmpty()
            .WithMessage("Installation date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Installation date cannot be in the future.");

        RuleFor(x => x.Multiplier)
            .GreaterThan(0)
            .WithMessage("Multiplier must be greater than 0.");

        RuleFor(x => x.CommunicationProtocol)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.CommunicationProtocol))
            .WithMessage("Communication protocol cannot exceed 50 characters.");
    }
}

