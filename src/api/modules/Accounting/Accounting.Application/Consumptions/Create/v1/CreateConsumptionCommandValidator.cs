namespace Accounting.Application.Consumptions.Create.v1;

/// <summary>
/// Validator for CreateConsumptionCommand.
/// </summary>
public sealed class CreateConsumptionCommandValidator : AbstractValidator<CreateConsumptionCommand>
{
    public CreateConsumptionCommandValidator()
    {
        RuleFor(x => x.MeterId)
            .NotEmpty()
            .WithMessage("Meter ID is required.");

        RuleFor(x => x.ReadingDate)
            .NotEmpty()
            .WithMessage("Reading date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Reading date cannot be in the future.");

        RuleFor(x => x.CurrentReading)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Current reading must be non-negative.");

        RuleFor(x => x.PreviousReading)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Previous reading must be non-negative.");

        RuleFor(x => x.BillingPeriod)
            .NotEmpty()
            .WithMessage("Billing period is required.")
            .MaximumLength(64)
            .WithMessage("Billing period cannot exceed 64 characters.");

        RuleFor(x => x.ReadingType)
            .Must(type => new[] { "Actual", "Estimated", "Customer Read" }
                .Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid reading type. Must be: Actual, Estimated, or Customer Read.");

        RuleFor(x => x.Multiplier)
            .GreaterThan(0)
            .When(x => x.Multiplier.HasValue)
            .WithMessage("Multiplier must be greater than 0 if provided.");
    }
}

