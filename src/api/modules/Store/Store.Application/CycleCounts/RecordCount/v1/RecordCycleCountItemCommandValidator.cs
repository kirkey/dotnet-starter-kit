namespace FSH.Starter.WebApi.Store.Application.CycleCounts.RecordCount.v1;

/// <summary>
/// Validator for RecordCycleCountItemCommand with strict validation rules.
/// </summary>
public sealed class RecordCycleCountItemCommandValidator : AbstractValidator<RecordCycleCountItemCommand>
{
    public RecordCycleCountItemCommandValidator()
    {
        RuleFor(x => x.CycleCountId)
            .NotEmpty()
            .WithMessage("CycleCountId is required");

        RuleFor(x => x.CycleCountItemId)
            .NotEmpty()
            .WithMessage("CycleCountItemId is required");

        RuleFor(x => x.CountedQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("CountedQuantity must be zero or greater");

        RuleFor(x => x.CountedBy)
            .MaximumLength(128)
            .When(x => !string.IsNullOrWhiteSpace(x.CountedBy))
            .WithMessage("CountedBy must not exceed 100 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must not exceed 500 characters");
    }
}

