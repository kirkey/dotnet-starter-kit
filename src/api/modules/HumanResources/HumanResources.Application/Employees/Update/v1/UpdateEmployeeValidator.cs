namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.FirstName)
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Middle name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Status)
            .Must(BeValidStatus).WithMessage("Status must be one of: Active, OnLeave, Terminated.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }

    private static bool BeValidStatus(string status)
    {
        return status is "Active" or "OnLeave" or "Terminated";
    }
}

