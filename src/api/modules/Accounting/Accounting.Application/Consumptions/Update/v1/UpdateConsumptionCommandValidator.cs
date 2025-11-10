namespace Accounting.Application.Consumptions.Update.v1;

/// <summary>
/// Validator for UpdateConsumptionCommand.
/// </summary>
public sealed class UpdateConsumptionCommandValidator : AbstractValidator<UpdateConsumptionCommand>
{
    public UpdateConsumptionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Consumption ID is required.");

        RuleFor(x => x.ReadingType)
            .Must(type => new[] { "Actual", "Estimated", "Customer Read" }
                .Contains(type!, StringComparer.OrdinalIgnoreCase))
            .When(x => !string.IsNullOrWhiteSpace(x.ReadingType))
            .WithMessage("Invalid reading type. Must be: Actual, Estimated, or Customer Read.");
    }
}

