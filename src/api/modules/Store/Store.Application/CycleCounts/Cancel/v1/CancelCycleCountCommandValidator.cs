namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Cancel.v1;

/// <summary>
/// Validator for CancelCycleCountCommand with strict validation rules.
/// </summary>
public sealed class CancelCycleCountCommandValidator : AbstractValidator<CancelCycleCountCommand>
{
    public CancelCycleCountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("CycleCount ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Cancellation reason is required")
            .MinimumLength(5)
            .WithMessage("Cancellation reason must be at least 5 characters")
            .MaximumLength(500)
            .WithMessage("Cancellation reason must not exceed 500 characters");
    }
}

