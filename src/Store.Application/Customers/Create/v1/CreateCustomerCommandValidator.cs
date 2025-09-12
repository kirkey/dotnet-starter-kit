namespace FSH.Starter.WebApi.Store.Application.Customers.Create.v1;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Customer code must contain only uppercase letters and numbers");

        RuleFor(x => x.CustomerType)
            .NotEmpty()
            .Must(type => new[] { "Retail", "Wholesale", "Corporate" }.Contains(type))
            .WithMessage("Customer type must be Retail, Wholesale, or Corporate");

        RuleFor(x => x.ContactPerson)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PaymentTermsDays)
            .GreaterThan(0)
            .LessThanOrEqualTo(365);

        RuleFor(x => x.DiscountPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100);
    }
}
