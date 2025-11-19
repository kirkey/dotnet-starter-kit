using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;

/// <summary>
/// Validator for updating an employee dependent.
/// </summary>
public class UpdateEmployeeDependentValidator : AbstractValidator<UpdateEmployeeDependentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEmployeeDependentValidator"/> class.
    /// </summary>
    public UpdateEmployeeDependentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Dependent ID is required");

        RuleFor(x => x.FirstName)
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

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
}
