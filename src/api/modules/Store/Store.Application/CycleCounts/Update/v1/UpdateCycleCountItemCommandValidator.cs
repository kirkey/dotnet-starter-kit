namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

/// <summary>
/// Validator for UpdateCycleCountItemCommand.
/// </summary>
public sealed class UpdateCycleCountItemCommandValidator : AbstractValidator<UpdateCycleCountItemCommand>
{
    public UpdateCycleCountItemCommandValidator()
    {
        RuleFor(x => x.ActualQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Actual quantity must be zero or greater.");

        RuleFor(x => x.Notes)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes cannot exceed 1000 characters.");
    }
}

