namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;

/// <summary>
/// Validator for CreateDesignationCommand.
/// </summary>
public class CreateDesignationValidator : AbstractValidator<CreateDesignationCommand>
{
    public CreateDesignationValidator()
    {
        RuleFor(x => x.OrganizationalUnitId)
            .NotEmpty().WithMessage("Organizational unit ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
            .Matches(@"^[A-Z0-9-]+$").WithMessage("Code must contain only uppercase letters, numbers, and hyphens.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.MinSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum salary must be greater than or equal to 0.")
            .When(x => x.MinSalary.HasValue);

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum salary must be greater than or equal to 0.")
            .When(x => x.MaxSalary.HasValue);

        RuleFor(x => x.MaxSalary)
            .GreaterThanOrEqualTo(x => x.MinSalary)
            .WithMessage("Maximum salary must be greater than or equal to minimum salary.")
            .When(x => x.MinSalary.HasValue && x.MaxSalary.HasValue);
    }
}

