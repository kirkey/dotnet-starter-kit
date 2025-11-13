namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

/// <summary>
/// Validator for CreateEmployeeContactCommand.
/// </summary>
public class CreateEmployeeContactValidator : AbstractValidator<CreateEmployeeContactCommand>
{
    public CreateEmployeeContactValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(256).WithMessage("First name must not exceed 256 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(256).WithMessage("Last name must not exceed 256 characters.");

        RuleFor(x => x.ContactType)
            .NotEmpty().WithMessage("Contact type is required.")
            .Must(BeValidContactType).WithMessage("Contact type must be one of: Emergency, NextOfKin, Reference, Family.");

        RuleFor(x => x.Relationship)
            .MaximumLength(100).WithMessage("Relationship must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Relationship));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }

    private static bool BeValidContactType(string contactType)
    {
        return contactType is "Emergency" or "NextOfKin" or "Reference" or "Family";
    }
}

