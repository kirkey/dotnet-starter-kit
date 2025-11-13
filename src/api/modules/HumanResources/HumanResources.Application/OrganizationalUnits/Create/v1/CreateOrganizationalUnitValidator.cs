namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Create.v1;

/// <summary>
/// Validator for CreateOrganizationalUnitCommand.
/// </summary>
public class CreateOrganizationalUnitValidator : AbstractValidator<CreateOrganizationalUnitCommand>
{
    public CreateOrganizationalUnitValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
            .Matches(@"^[A-Z0-9-]+$").WithMessage("Code must contain only uppercase letters, numbers, and hyphens.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid organizational unit type.");

        // Department cannot have parent
        RuleFor(x => x.ParentId)
            .Null()
            .When(x => x.Type == OrganizationalUnitType.Department)
            .WithMessage("Department cannot have a parent.");

        // Division must have Department parent
        RuleFor(x => x.ParentId)
            .NotNull()
            .When(x => x.Type == OrganizationalUnitType.Division)
            .WithMessage("Division must have a Department parent.");

        // Section must have Division parent
        RuleFor(x => x.ParentId)
            .NotNull()
            .When(x => x.Type == OrganizationalUnitType.Section)
            .WithMessage("Section must have a Division parent.");

        RuleFor(x => x.CostCenter)
            .MaximumLength(50).WithMessage("Cost center must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CostCenter));

        RuleFor(x => x.Location)
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Location));
    }
}

