namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;

/// <summary>
/// Validator for UpdateEmployeeDependentCommand.
/// </summary>
public class UpdateEmployeeDependentValidator : AbstractValidator<UpdateEmployeeDependentCommand>
{
    public UpdateEmployeeDependentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.FirstName)
            .MaximumLength(256).WithMessage("First name must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .MaximumLength(256).WithMessage("Last name must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

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

        RuleFor(x => x.EligibilityEndDate)
            .GreaterThan(DateTime.Today).WithMessage("Eligibility end date must be in the future.")
            .When(x => x.EligibilityEndDate.HasValue);
    }
}

