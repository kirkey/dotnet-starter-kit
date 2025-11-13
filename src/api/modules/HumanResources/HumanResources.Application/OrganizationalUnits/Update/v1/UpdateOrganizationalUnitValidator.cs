namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Update.v1;

/// <summary>
/// Validator for UpdateOrganizationalUnitCommand.
/// </summary>
public class UpdateOrganizationalUnitValidator : AbstractValidator<UpdateOrganizationalUnitCommand>
{
    public UpdateOrganizationalUnitValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");

        RuleFor(x => x.CostCenter)
            .MaximumLength(50).WithMessage("Cost center must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CostCenter));

        RuleFor(x => x.Location)
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Location));
    }
}

