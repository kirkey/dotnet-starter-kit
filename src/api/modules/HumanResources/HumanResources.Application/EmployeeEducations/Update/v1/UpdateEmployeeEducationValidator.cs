namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Update.v1;

/// <summary>
/// Validator for updating an employee education record.
/// </summary>
public class UpdateEmployeeEducationValidator : AbstractValidator<UpdateEmployeeEducationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEmployeeEducationValidator"/> class.
    /// </summary>
    public UpdateEmployeeEducationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Education record ID is required");

        RuleFor(x => x.FieldOfStudy)
            .MaximumLength(100)
            .WithMessage("Field of study cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.FieldOfStudy));

        RuleFor(x => x.Degree)
            .MaximumLength(100)
            .WithMessage("Degree cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Degree));

        RuleFor(x => x.Gpa)
            .GreaterThanOrEqualTo(0)
            .WithMessage("GPA cannot be negative")
            .LessThanOrEqualTo(4.0m)
            .WithMessage("GPA cannot exceed 4.0")
            .When(x => x.Gpa.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

