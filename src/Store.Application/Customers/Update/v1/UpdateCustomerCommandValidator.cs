using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.Store.Application.Customers.Update.v1;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    private static readonly Regex PhoneRegex = new(@"^[0-9+()\-\s]{5,50}$", RegexOptions.Compiled);

    public UpdateCustomerCommandValidator([FromKeyedServices("store:customers")] IReadRepository<Customer> repository)
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .When(x => x.Name is not null);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .When(x => x.Description is not null);

        RuleFor(x => x.Code)
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9-]+$")
            .WithMessage("Customer code must contain only uppercase letters, numbers, and hyphens")
            .When(x => x.Code is not null)
            .MustAsync(async (cmd, code, ct) =>
            {
                if (string.IsNullOrWhiteSpace(code)) return true;
                var existing = await repository.FirstOrDefaultAsync(new Specs.CustomerByCodeSpec(code), ct).ConfigureAwait(false);
                return existing is null || existing.Id == cmd.Id;
            }).WithMessage("Customer code must be unique");

        RuleFor(x => x.CustomerType)
            .Must(type => new[] { "Retail", "Wholesale", "Corporate" }.Contains(type!))
            .WithMessage("Customer type must be Retail, Wholesale, or Corporate")
            .When(x => x.CustomerType is not null);

        RuleFor(x => x.ContactPerson)
            .MaximumLength(100)
            .When(x => x.ContactPerson is not null);

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(255)
            .When(x => x.Email is not null)
            .MustAsync(async (cmd, email, ct) =>
            {
                if (string.IsNullOrWhiteSpace(email)) return true;
                var existing = await repository.FirstOrDefaultAsync(new Specs.CustomerByEmailSpec(email), ct).ConfigureAwait(false);
                return existing is null || existing.Id == cmd.Id;
            }).WithMessage("A customer with the same email already exists");

        RuleFor(x => x.Phone)
            .MaximumLength(50)
            .Matches(PhoneRegex).WithMessage("Invalid phone number format")
            .When(x => x.Phone is not null);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .When(x => x.Address is not null);

        RuleFor(x => x.City)
            .MaximumLength(100)
            .When(x => x.City is not null);

        RuleFor(x => x.State)
            .MaximumLength(100)
            .When(x => x.State is not null);

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .When(x => x.Country is not null);

        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .When(x => x.PostalCode is not null);

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PaymentTermsDays)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(365);

        RuleFor(x => x.DiscountPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.TaxNumber)
            .MaximumLength(50)
            .When(x => x.TaxNumber is not null);

        RuleFor(x => x.BusinessLicense)
            .MaximumLength(100)
            .When(x => x.BusinessLicense is not null);

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .When(x => x.Notes is not null);
    }
}
