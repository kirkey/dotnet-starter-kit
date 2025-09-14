using Accounting.Application.ConsumptionData.Commands;

namespace Accounting.Application.ConsumptionData.Validators;

public class CreateConsumptionDataCommandValidator : AbstractValidator<CreateConsumptionDataCommand>
{
    public CreateConsumptionDataCommandValidator()
    {
        RuleFor(x => x.MeterId).NotEmpty();
        RuleFor(x => x.ReadingDate).NotEmpty();
        RuleFor(x => x.CurrentReading).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PreviousReading).GreaterThanOrEqualTo(0);
        RuleFor(x => x.BillingPeriod).NotEmpty().MaximumLength(64);
        RuleFor(x => x.ReadingType).MaximumLength(32).When(x => !string.IsNullOrEmpty(x.ReadingType));
        RuleFor(x => x.Multiplier).GreaterThan(0).When(x => x.Multiplier.HasValue);
        RuleFor(x => x.ReadingSource).MaximumLength(32).When(x => !string.IsNullOrEmpty(x.ReadingSource));
        RuleFor(x => x.Description).MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Description));
        RuleFor(x => x.Notes).MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

