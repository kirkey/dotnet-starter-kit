using System.Text.RegularExpressions;
using FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

/// <summary>
/// Validator for updating a Supplier. Applies strict length/format checks and ensures email uniqueness when changed.
/// </summary>
public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex PhoneRegex = new(@"^[0-9+()\-\s]{5,64}$", RegexOptions.Compiled);

    public UpdateSupplierCommandValidator([FromKeyedServices("store:suppliers")] IReadRepository<Supplier> repository)
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Code).MaximumLength(32).When(x => !string.IsNullOrEmpty(x.Name));
        RuleFor(x => x.Name).MaximumLength(256).When(x => !string.IsNullOrEmpty(x.Name));
        RuleFor(x => x.ContactPerson).MaximumLength(128).When(x => !string.IsNullOrEmpty(x.ContactPerson));

        RuleFor(x => x.Email)
            .MaximumLength(256)
            .Matches(EmailRegex)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email address")
            .MustAsync(async (cmd, email, ct) =>
            {
                if (string.IsNullOrWhiteSpace(email)) return true;
                var existing = await repository.FirstOrDefaultAsync(new SupplierByEmailSpec(email), ct).ConfigureAwait(false);
                return existing is null || existing.Id == cmd.Id;
            }).WithMessage("A supplier with the same email already exists");

        RuleFor(x => x.Phone)
            .MaximumLength(64)
            .Matches(PhoneRegex)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.Address).MaximumLength(512).When(x => !string.IsNullOrEmpty(x.Address));
        RuleFor(x => x.PostalCode).MaximumLength(32).When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.Website)
            .MaximumLength(256)
            .Must(uri => Uri.TryCreate(uri!, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrWhiteSpace(x.Website))
            .WithMessage("Website must be a valid absolute URL");

        RuleFor(x => x.CreditLimit).GreaterThanOrEqualTo(0).When(x => x.CreditLimit.HasValue);
        RuleFor(x => x.PaymentTermsDays).GreaterThanOrEqualTo(0).LessThanOrEqualTo(365).When(x => x.PaymentTermsDays.HasValue);
        RuleFor(x => x.Rating).InclusiveBetween(0m, 5m).When(x => x.Rating.HasValue);
        RuleFor(x => x.Notes).MaximumLength(2048).When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
