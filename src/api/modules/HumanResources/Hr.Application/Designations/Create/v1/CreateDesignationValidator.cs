namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;

/// <summary>
/// Validator for CreateDesignationCommand with area-specific configuration.
/// </summary>
public class CreateDesignationValidator : AbstractValidator<CreateDesignationCommand>
{
    private static readonly string[] ValidAreas = { "Metro Manila", "Visayas", "Mindanao", "Luzon", "National" };
    private static readonly string[] ValidGrades = { "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Executive" };

    public CreateDesignationValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
            .Matches(@"^[A-Z0-9-]+$").WithMessage("Code must contain only uppercase letters, numbers, and hyphens.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

        RuleFor(x => x.Area)
            .NotEmpty().WithMessage("Area is required.")
            .Must(x => ValidAreas.Contains(x)).WithMessage($"Area must be one of: {string.Join(", ", ValidAreas)}")
            .When(x => !string.IsNullOrWhiteSpace(x.Area));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.SalaryGrade)
            .Must(x => ValidGrades.Contains(x)).WithMessage($"Salary Grade must be one of: {string.Join(", ", ValidGrades)}")
            .When(x => !string.IsNullOrWhiteSpace(x.SalaryGrade));

        RuleFor(x => x.MinimumSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum salary must be greater than or equal to 0.")
            .When(x => x.MinimumSalary.HasValue);

        RuleFor(x => x.MaximumSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum salary must be greater than or equal to 0.")
            .When(x => x.MaximumSalary.HasValue);

        RuleFor(x => x.MaximumSalary)
            .GreaterThanOrEqualTo(x => x.MinimumSalary)
            .WithMessage("Maximum salary must be greater than or equal to minimum salary.")
            .When(x => x.MinimumSalary.HasValue && x.MaximumSalary.HasValue);
    }
}

