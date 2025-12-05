namespace Accounting.Application.Checks.Clear.v1;

/// <summary>
/// Validator for clear check command.
/// </summary>
public class ClearCheckCommandValidator : AbstractValidator<ClearCheckCommand>
{
    public ClearCheckCommandValidator()
    {
        RuleFor(x => x.CheckId)
            .NotEmpty().WithMessage("Check ID is required.");

        RuleFor(x => x.ClearedDate)
            .NotEmpty().WithMessage("Cleared date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Cleared date cannot be in the future.");
    }
}

