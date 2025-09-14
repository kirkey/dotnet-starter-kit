using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    private static readonly string[] AllowedCountries = new[] { "" }; // placeholder - allow any for now
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ContactPerson).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(255).Must(e => EmailRegex.IsMatch(e)).WithMessage("Invalid email address");
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PostalCode).MaximumLength(20).When(x => !string.IsNullOrEmpty(x.PostalCode));
        RuleFor(x => x.Website).MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Website));
        RuleFor(x => x.CreditLimit).GreaterThanOrEqualTo(0).When(x => x.CreditLimit.HasValue);
        RuleFor(x => x.PaymentTermsDays).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Rating).InclusiveBetween(0m, 5m);
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Notes));
        // Code uniqueness check could be added here if repository available
    }
}

