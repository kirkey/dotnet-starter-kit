using System.Text.RegularExpressions;

namespace FSH.Starter.WebApi.Store.Application.Customers.Create.v1;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private static readonly Regex PhoneRegex = new(@"^[0-9+()\-\s]{5,50}$", RegexOptions.Compiled);

    public CreateCustomerCommandValidator([FromKeyedServices("store:customers")] IReadRepository<Customer> repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9-]+$")
            .WithMessage("Customer code must contain only uppercase letters, numbers, and hyphens")
            .MustAsync(async (code, ct) =>
            {
                var existing = await repository.FirstOrDefaultAsync(new Specs.CustomerByCodeSpec(code!), ct).ConfigureAwait(false);
                return existing is null;
            }).WithMessage("Customer code must be unique");

        RuleFor(x => x.CustomerType)
            .NotEmpty()
            .Must(type => new[] { "Retail", "Wholesale", "Corporate" }.Contains(type!))
            .WithMessage("Customer type must be Retail, Wholesale, or Corporate");

        RuleFor(x => x.ContactPerson)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .MustAsync(async (email, ct) =>
            {
                var existing = await repository.FirstOrDefaultAsync(new Specs.CustomerByEmailSpec(email!), ct).ConfigureAwait(false);
                return existing is null;
            }).WithMessage("A customer with the same email already exists");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(PhoneRegex).WithMessage("Invalid phone number format");

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PaymentTermsDays)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(365);

        RuleFor(x => x.DiscountPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
