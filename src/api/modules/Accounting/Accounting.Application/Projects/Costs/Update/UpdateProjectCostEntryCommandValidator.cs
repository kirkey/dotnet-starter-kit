namespace Accounting.Application.Projects.Costs.Update;

/// <summary>
/// Validator for <see cref="UpdateProjectCostEntryCommand"/>.
/// </summary>
public sealed class UpdateProjectCostEntryCommandValidator : AbstractValidator<UpdateProjectCostEntryCommand>
{
    public UpdateProjectCostEntryCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.EntryId).NotEmpty();
        RuleFor(x => x.Description)
            .MaximumLength(512);
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .When(x => x.Amount.HasValue);
        RuleFor(x => x.Category)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Category));
    }
}
