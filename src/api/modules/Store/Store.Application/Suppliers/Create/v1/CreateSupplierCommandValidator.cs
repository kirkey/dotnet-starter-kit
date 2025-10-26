using System.Text.RegularExpressions;
using FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;

/// <summary>
/// Validator for creating a Supplier. Enforces uniqueness and strict formatting rules.
/// </summary>
public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex PhoneRegex = new(@"^[0-9+()\-\s]{5,50}$", RegexOptions.Compiled);

    public CreateSupplierCommandValidator([FromKeyedServices("store:suppliers")] IReadRepository<Supplier> repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9-]+$")
            .WithMessage("Code must contain only uppercase letters, numbers, and hyphens")
            .MustAsync(async (code, ct) =>
            {
                // ensure no existing supplier has same code
                var exists = await repository.FirstOrDefaultAsync(new SupplierByCodeSpec(code), ct).ConfigureAwait(false);
                return exists is null;
            }).WithMessage("Supplier code must be unique");

        RuleFor(x => x.ContactPerson)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(255)
            .Must(e => EmailRegex.IsMatch(e)).WithMessage("Invalid email address")
            .MustAsync(async (email, ct) =>
            {
                var exists = await repository.FirstOrDefaultAsync(new SupplierByEmailSpec(email), ct).ConfigureAwait(false);
                return exists is null;
            }).WithMessage("A supplier with the same email already exists");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(PhoneRegex).WithMessage("Invalid phone number format");

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);


        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.Website)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.Website))
            .Must(uri => Uri.TryCreate(uri!, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrWhiteSpace(x.Website))
            .WithMessage("Website must be a valid absolute URL");

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0)
            .When(x => x.CreditLimit.HasValue);

        RuleFor(x => x.PaymentTermsDays)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(365);

        RuleFor(x => x.Rating)
            .InclusiveBetween(0m, 5m);

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

// removed inline spec; now defined under Suppliers/Specs
