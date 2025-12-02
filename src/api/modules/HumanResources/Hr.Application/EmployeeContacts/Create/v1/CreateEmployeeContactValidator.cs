using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

/// <summary>
/// Validator for creating an employee contact.
/// </summary>
public class CreateEmployeeContactValidator : AbstractValidator<CreateEmployeeContactCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEmployeeContactValidator"/> class.
    /// </summary>
    public CreateEmployeeContactValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(128)
            .WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(128)
            .WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.ContactType)
            .NotEmpty()
            .WithMessage("Contact type is required")
            .Must(BeValidContactType)
            .WithMessage("Contact type must be Emergency, NextOfKin, Reference, or Family");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(32)
            .WithMessage("Phone number cannot exceed 20 characters")
            .Matches(@"^\+?[0-9\s\-\(\)]*$", RegexOptions.IgnoreCase)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number format is invalid");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email format is invalid");

        RuleFor(x => x.Address)
            .MaximumLength(256)
            .WithMessage("Address cannot exceed 250 characters");
    }

    /// <summary>
    /// Validates if the contact type is valid.
    /// </summary>
    private static bool BeValidContactType(string? contactType)
    {
        if (string.IsNullOrWhiteSpace(contactType))
            return false;

        var validTypes = new[] { "Emergency", "NextOfKin", "Reference", "Family" };
        return validTypes.Contains(contactType);
    }
}

