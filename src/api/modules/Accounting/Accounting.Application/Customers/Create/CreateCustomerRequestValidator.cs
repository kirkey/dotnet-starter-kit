namespace Accounting.Application.Customers.Create;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.CustomerCode)
            .NotEmpty()
            .MaximumLength(16);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.ContactPerson)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.ContactPerson));

        RuleFor(x => x.Terms)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Terms));

        RuleFor(x => x.RevenueAccountCode)
            .MaximumLength(16)
            .When(x => !string.IsNullOrEmpty(x.RevenueAccountCode));

        RuleFor(x => x.RevenueAccountName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.RevenueAccountName));

        RuleFor(x => x.Tin)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Tin));

        RuleFor(x => x.BillingAddress)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.BillingAddress));

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
