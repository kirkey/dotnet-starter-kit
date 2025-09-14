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
    }
}
