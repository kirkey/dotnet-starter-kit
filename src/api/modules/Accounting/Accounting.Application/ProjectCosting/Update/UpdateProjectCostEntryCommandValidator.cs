namespace Accounting.Application.ProjectCosting.Update;

/// <summary>
/// Validator for <see cref="UpdateProjectCostEntryCommand"/>.
/// </summary>
public sealed class UpdateProjectCostEntryCommandValidator : AbstractValidator<UpdateProjectCostEntryCommand>
{
    public UpdateProjectCostEntryCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.EntryId).NotEmpty();
        RuleFor(x => x.Date)
            .Must(d => !d.HasValue || d.Value.Date <= DateTime.UtcNow.Date)
            .WithMessage("Date cannot be in the future.");
        RuleFor(x => x.Description)
            .MinimumLength(3)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .When(x => x.Amount.HasValue);
        RuleFor(x => x.Category)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Category));
    }
}
