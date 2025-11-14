namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Validator for creating a deduction.
/// </summary>
public class CreateDeductionValidator : AbstractValidator<CreateDeductionCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDeductionValidator"/> class.
    /// </summary>
    public CreateDeductionValidator()
    {
        RuleFor(x => x.ComponentName)
            .NotEmpty()
            .WithMessage("Component name is required")
            .MaximumLength(100)
            .WithMessage("Component name cannot exceed 100 characters");

        RuleFor(x => x.ComponentType)
            .NotEmpty()
            .WithMessage("Component type is required")
            .Must(BeValidComponentType)
            .WithMessage("Component type must be Earnings, Tax, or Deduction");

        RuleFor(x => x.GlAccountCode)
            .MaximumLength(20)
            .WithMessage("GL account code cannot exceed 20 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.GlAccountCode));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }

    private static bool BeValidComponentType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return false;

        var validTypes = new[] { "Earnings", "Tax", "Deduction" };
        return validTypes.Contains(type);
    }
}

