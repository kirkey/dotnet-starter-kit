namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Create.v1;

/// <summary>
/// Validator for creating an employee education record.
/// </summary>
public class CreateEmployeeEducationValidator : AbstractValidator<CreateEmployeeEducationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmployeeEducationValidator"/> class.
    /// </summary>
    public CreateEmployeeEducationValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.EducationLevel)
            .NotEmpty()
            .WithMessage("Education level is required")
            .MaximumLength(50)
            .WithMessage("Education level cannot exceed 50 characters");

        RuleFor(x => x.FieldOfStudy)
            .NotEmpty()
            .WithMessage("Field of study is required")
            .MaximumLength(100)
            .WithMessage("Field of study cannot exceed 100 characters");

        RuleFor(x => x.Institution)
            .NotEmpty()
            .WithMessage("Institution name is required")
            .MaximumLength(150)
            .WithMessage("Institution name cannot exceed 150 characters");

        RuleFor(x => x.GraduationDate)
            .NotEmpty()
            .WithMessage("Graduation date is required")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Graduation date cannot be in the future");

        RuleFor(x => x.Gpa)
            .GreaterThanOrEqualTo(0)
            .WithMessage("GPA cannot be negative")
            .LessThanOrEqualTo(4.0m)
            .WithMessage("GPA cannot exceed 4.0")
            .When(x => x.Gpa.HasValue);

        RuleFor(x => x.Degree)
            .MaximumLength(100)
            .WithMessage("Degree cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Degree));

        RuleFor(x => x.CertificateNumber)
            .MaximumLength(50)
            .WithMessage("Certificate number cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.CertificateNumber));

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

