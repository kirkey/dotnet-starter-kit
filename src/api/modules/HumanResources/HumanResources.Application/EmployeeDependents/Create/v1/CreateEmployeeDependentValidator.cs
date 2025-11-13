namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

/// <summary>
/// Validator for CreateEmployeeDependentCommand.
/// </summary>
public class CreateEmployeeDependentValidator : AbstractValidator<CreateEmployeeDependentCommand>
{
    public CreateEmployeeDependentValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(256).WithMessage("First name must not exceed 256 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(256).WithMessage("Last name must not exceed 256 characters.");

        RuleFor(x => x.DependentType)
            .NotEmpty().WithMessage("Dependent type is required.")
            .Must(BeValidDependentType).WithMessage("Dependent type must be one of: Spouse, Child, Parent, Sibling, Other.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Relationship)
            .MaximumLength(100).WithMessage("Relationship must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Relationship));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }

    private static bool BeValidDependentType(string dependentType)
    {
        return dependentType is "Spouse" or "Child" or "Parent" or "Sibling" or "Other";
    }
}

