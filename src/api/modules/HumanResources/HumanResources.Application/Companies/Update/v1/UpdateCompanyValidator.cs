namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Update.v1;

/// <summary>
/// Validator for UpdateCompanyCommand.
/// </summary>
public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");

        RuleFor(x => x.TIN)
            .MaximumLength(50).WithMessage("TIN must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.TIN));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.ZipCode)
            .MaximumLength(20).WithMessage("Zip code must not exceed 20 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ZipCode));

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("Phone must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Email)
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.")
            .EmailAddress().WithMessage("Email must be valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Website)
            .MaximumLength(500).WithMessage("Website must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));
    }
}

