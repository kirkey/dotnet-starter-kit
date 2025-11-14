namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

/// <summary>
/// Validator for CreateEmployeeCommand with Philippines Labor Code compliance rules.
/// Enforces mandatory government IDs format, age requirements, and employment classification.
/// </summary>
public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        // Basic Required Fields
        RuleFor(x => x.EmployeeNumber)
            .NotEmpty().WithMessage("Employee number is required.")
            .MaximumLength(50).WithMessage("Employee number must not exceed 50 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Middle name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.OrganizationalUnitId)
            .NotEmpty().WithMessage("Organizational unit ID is required.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .Matches(@"^\+?639\d{9}$").WithMessage("Phone number must be valid Philippines mobile format (+639XXXXXXXXX).")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Hire date cannot be in the future.")
            .When(x => x.HireDate.HasValue);

        // Philippines-Specific: Birth Date Validation (Minimum Age 18)
        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today.AddYears(-18)).WithMessage("Employee must be at least 18 years old per Labor Code.")
            .GreaterThan(DateTime.Today.AddYears(-70)).WithMessage("Birth date seems invalid (over 70 years ago).")
            .When(x => x.BirthDate.HasValue);

        // Philippines-Specific: Gender Validation
        RuleFor(x => x.Gender)
            .Must(g => g == "Male" || g == "Female").WithMessage("Gender must be 'Male' or 'Female'.")
            .When(x => !string.IsNullOrWhiteSpace(x.Gender));

        // Philippines-Specific: Civil Status Validation
        RuleFor(x => x.CivilStatus)
            .Must(cs => new[] { "Single", "Married", "Widowed", "Separated", "Divorced" }.Contains(cs))
            .WithMessage("Civil status must be one of: Single, Married, Widowed, Separated, Divorced.")
            .When(x => !string.IsNullOrWhiteSpace(x.CivilStatus));

        // Philippines-Specific: TIN Validation (Format: XXX-XXX-XXX-XXX)
        RuleFor(x => x.Tin)
            .Matches(@"^\d{3}-\d{3}-\d{3}-\d{3}$")
            .WithMessage("TIN must be in format XXX-XXX-XXX-XXX (e.g., 123-456-789-000).")
            .When(x => !string.IsNullOrWhiteSpace(x.Tin));

        // Philippines-Specific: SSS Number Validation (Format: XX-XXXXXXX-X)
        RuleFor(x => x.SssNumber)
            .Matches(@"^\d{2}-\d{7}-\d{1}$")
            .WithMessage("SSS Number must be in format XX-XXXXXXX-X (e.g., 34-1234567-8).")
            .When(x => !string.IsNullOrWhiteSpace(x.SssNumber));

        // Philippines-Specific: PhilHealth Number Validation (Format: XX-XXXXXXXXX-X)
        RuleFor(x => x.PhilHealthNumber)
            .Matches(@"^\d{2}-\d{9}-\d{1}$")
            .WithMessage("PhilHealth Number must be in format XX-XXXXXXXXX-X (e.g., 12-345678901-2).")
            .When(x => !string.IsNullOrWhiteSpace(x.PhilHealthNumber));

        // Philippines-Specific: Pag-IBIG Number Validation (Format: XXXX-XXXX-XXXX)
        RuleFor(x => x.PagIbigNumber)
            .Matches(@"^\d{4}-\d{4}-\d{4}$")
            .WithMessage("Pag-IBIG Number must be in format XXXX-XXXX-XXXX (e.g., 1234-5678-9012).")
            .When(x => !string.IsNullOrWhiteSpace(x.PagIbigNumber));

        // Philippines-Specific: Employment Classification per Labor Code Article 280
        RuleFor(x => x.EmploymentClassification)
            .Must(ec => new[] { "Regular", "Probationary", "Casual", "ProjectBased", "Seasonal", "Contractual" }.Contains(ec))
            .WithMessage("Employment classification must be: Regular, Probationary, Casual, ProjectBased, Seasonal, or Contractual per Labor Code Article 280.");

        // Philippines-Specific: Regularization Date Validation
        RuleFor(x => x.RegularizationDate)
            .GreaterThanOrEqualTo(x => x.HireDate)
            .WithMessage("Regularization date must be on or after hire date.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Regularization date cannot be in the future.")
            .When(x => x.RegularizationDate.HasValue && x.HireDate.HasValue);

        // Philippines-Specific: Basic Monthly Salary Validation
        RuleFor(x => x.BasicMonthlySalary)
            .GreaterThan(0).WithMessage("Basic monthly salary must be greater than zero.")
            .LessThan(1000000).WithMessage("Basic monthly salary seems unreasonably high.")
            .When(x => x.BasicMonthlySalary.HasValue);

        // Philippines-Specific: PWD ID Validation
        RuleFor(x => x.PwdIdNumber)
            .NotEmpty().WithMessage("PWD ID Number is required when IsPWD is true.")
            .MaximumLength(50).WithMessage("PWD ID Number must not exceed 50 characters.")
            .When(x => x.IsPwd);

        // Philippines-Specific: Solo Parent ID Validation
        RuleFor(x => x.SoloParentIdNumber)
            .NotEmpty().WithMessage("Solo Parent ID Number is required when IsSoloParent is true.")
            .MaximumLength(50).WithMessage("Solo Parent ID Number must not exceed 50 characters.")
            .When(x => x.IsSoloParent);
    }
}

