namespace FSH.Starter.WebApi.Store.Application.Customers.Update.v1;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => x.Name is not null);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Customer code must contain only uppercase letters and numbers")
            .When(x => x.Code is not null);

        RuleFor(x => x.CustomerType)
            .NotEmpty()
            .Must(type => new[] { "Retail", "Wholesale", "Corporate" }.Contains(type))
            .WithMessage("Customer type must be Retail, Wholesale, or Corporate")
            .When(x => x.CustomerType is not null);

        RuleFor(x => x.ContactPerson)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.ContactPerson is not null);

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(255)
            .When(x => x.Email is not null);

        RuleFor(x => x.Phone)
            .MaximumLength(50)
            .When(x => x.Phone is not null);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .When(x => x.Address is not null);

        RuleFor(x => x.City)
            .MaximumLength(100)
            .When(x => x.City is not null);

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .When(x => x.Country is not null);

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

