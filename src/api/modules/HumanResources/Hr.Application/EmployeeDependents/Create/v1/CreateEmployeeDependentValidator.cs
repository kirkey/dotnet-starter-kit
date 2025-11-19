using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

/// <summary>
/// Validator for creating an employee dependent.
/// </summary>
public class CreateEmployeeDependentValidator : AbstractValidator<CreateEmployeeDependentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmployeeDependentValidator"/> class.
    /// </summary>
    public CreateEmployeeDependentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.DependentType)
            .NotEmpty()
            .WithMessage("Dependent type is required")
            .Must(BeValidDependentType)
            .WithMessage("Dependent type must be Spouse, Child, Parent, Sibling, or Other");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of birth is required")
            .Must(BeValidDateOfBirth)
            .WithMessage("Date of birth cannot be in the future");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email format is invalid");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters")
            .Matches(@"^\+?[0-9\s\-\(\)]*$", RegexOptions.IgnoreCase)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number format is invalid");
    }

    /// <summary>
    /// Validates if the dependent type is valid.
    /// </summary>
    private static bool BeValidDependentType(string? dependentType)
    {
        if (string.IsNullOrWhiteSpace(dependentType))
            return false;

        var validTypes = new[] { "Spouse", "Child", "Parent", "Sibling", "Other" };
        return validTypes.Contains(dependentType);
    }

    /// <summary>
    /// Validates if the date of birth is not in the future.
    /// </summary>
    private static bool BeValidDateOfBirth(DateTime dateOfBirth)
    {
        return dateOfBirth <= DateTime.Today;
    }
}

