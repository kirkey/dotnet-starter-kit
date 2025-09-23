namespace Accounting.Application.ProjectCosting.Create;

/// <summary>
/// Validator for <see cref="CreateProjectCostEntryCommand"/>.
/// </summary>
public sealed class CreateProjectCostEntryCommandValidator : AbstractValidator<CreateProjectCostEntryCommand>
{
    public CreateProjectCostEntryCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Date)
            .NotEmpty()
            .Must(d => d.Date <= DateTime.UtcNow.Date)
            .WithMessage("Date cannot be in the future.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(512);
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .LessThanOrEqualTo(1_000_000_000m);
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.Category)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Category));
    }
}
