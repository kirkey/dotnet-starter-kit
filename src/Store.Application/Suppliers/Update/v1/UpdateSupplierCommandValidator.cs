using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public UpdateSupplierCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Name));
        RuleFor(x => x.ContactPerson).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.ContactPerson));
        RuleFor(x => x.Email).MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Email).Matches(EmailRegex).When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Invalid email address");
        RuleFor(x => x.Phone).MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Phone));
        RuleFor(x => x.Address).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Address));
        RuleFor(x => x.City).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.City));
        RuleFor(x => x.State).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.State));
        RuleFor(x => x.Country).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Country));
        RuleFor(x => x.PostalCode).MaximumLength(20).When(x => !string.IsNullOrEmpty(x.PostalCode));
        RuleFor(x => x.Website).MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Website));
        RuleFor(x => x.CreditLimit).GreaterThanOrEqualTo(0).When(x => x.CreditLimit.HasValue);
        RuleFor(x => x.PaymentTermsDays).GreaterThanOrEqualTo(0).When(x => x.PaymentTermsDays.HasValue);
        RuleFor(x => x.Rating).InclusiveBetween(0m, 5m).When(x => x.Rating.HasValue);
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

