namespace Accounting.Application.Customers.Update;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CustomerCode)
            .MaximumLength(16)
            .When(x => !string.IsNullOrEmpty(x.CustomerCode));

        RuleFor(x => x.Name)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Name));

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

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0)
            .When(x => x.CreditLimit.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
