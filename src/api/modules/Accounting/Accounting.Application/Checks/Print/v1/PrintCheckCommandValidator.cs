namespace Accounting.Application.Checks.Print.v1;

/// <summary>
/// Validator for print check command.
/// </summary>
public class PrintCheckCommandValidator : AbstractValidator<PrintCheckCommand>
{
    public PrintCheckCommandValidator()
    {
        RuleFor(x => x.CheckId)
            .NotEmpty().WithMessage("Check ID is required.");

        RuleFor(x => x.PrintedBy)
            .NotEmpty().WithMessage("Printed by user is required.")
            .MaximumLength(256).WithMessage("Printed by cannot exceed 256 characters.")
            .EmailAddress().WithMessage("Printed by should be a valid email address.");

        RuleFor(x => x.PrintedDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Printed date cannot be in the future.")
            .When(x => x.PrintedDate.HasValue);
    }
}

