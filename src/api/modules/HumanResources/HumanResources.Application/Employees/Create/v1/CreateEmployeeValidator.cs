namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
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
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Hire date cannot be in the future.")
            .When(x => x.HireDate.HasValue);
    }
}

