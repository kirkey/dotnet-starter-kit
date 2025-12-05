namespace Accounting.Application.Checks.Void.v1;

/// <summary>
/// Validator for void check command.
/// </summary>
public class VoidCheckCommandValidator : AbstractValidator<VoidCheckCommand>
{
    public VoidCheckCommandValidator()
    {
        RuleFor(x => x.CheckId)
            .NotEmpty().WithMessage("Check ID is required.");

        RuleFor(x => x.VoidReason)
            .NotEmpty().WithMessage("Void reason is required.")
            .MaximumLength(512).WithMessage("Void reason cannot exceed 512 characters.")
            .MinimumLength(5).WithMessage("Void reason must be at least 5 characters.");

        RuleFor(x => x.VoidedDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Voided date cannot be in the future.")
            .When(x => x.VoidedDate.HasValue);
    }
}

