using Accounting.Application.Consumptions.Commands;

namespace Accounting.Application.Consumptions.Validators;

public class UpdateConsumptionCommandValidator : AbstractValidator<UpdateConsumptionCommand>
{
    public UpdateConsumptionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CurrentReading).GreaterThanOrEqualTo(0).When(x => x.CurrentReading.HasValue);
        RuleFor(x => x.PreviousReading).GreaterThanOrEqualTo(0).When(x => x.PreviousReading.HasValue);
        RuleFor(x => x.ReadingType).MaximumLength(32).When(x => !string.IsNullOrEmpty(x.ReadingType));
        RuleFor(x => x.Multiplier).GreaterThan(0).When(x => x.Multiplier.HasValue);
        RuleFor(x => x.ReadingSource).MaximumLength(32).When(x => !string.IsNullOrEmpty(x.ReadingSource));
        RuleFor(x => x.Description).MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Description));
        RuleFor(x => x.Notes).MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

