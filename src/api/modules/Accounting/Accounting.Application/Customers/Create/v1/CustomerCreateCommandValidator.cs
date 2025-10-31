namespace Accounting.Application.Customers.Create.v1;

/// <summary>
/// Validator for customer creation command.
/// </summary>
public class CustomerCreateCommandValidator : AbstractValidator<CustomerCreateCommand>
{
    public CustomerCreateCommandValidator()
    {
        RuleFor(x => x.CustomerNumber)
            .NotEmpty().WithMessage("Customer number is required")
            .MaximumLength(50).WithMessage("Customer number cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("Customer number can only contain alphanumeric characters, hyphens, and underscores");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required")
            .MaximumLength(256).WithMessage("Customer name cannot exceed 256 characters");

        RuleFor(x => x.CustomerType)
            .NotEmpty().WithMessage("Customer type is required")
            .MaximumLength(32).WithMessage("Customer type cannot exceed 32 characters")
            .Must(type => new[] { "Individual", "Business", "Government", "NonProfit", "Wholesale", "Retail" }.Contains(type))
            .WithMessage("Customer type must be one of: Individual, Business, Government, NonProfit, Wholesale, Retail");

        RuleFor(x => x.BillingAddress)
            .NotEmpty().WithMessage("Billing address is required")
            .MaximumLength(500).WithMessage("Billing address cannot exceed 500 characters");

        RuleFor(x => x.ShippingAddress)
            .MaximumLength(500).WithMessage("Shipping address cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ShippingAddress));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("Phone cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.ContactName)
            .MaximumLength(256).WithMessage("Contact name cannot exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactName));

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Credit limit cannot be negative");

        RuleFor(x => x.PaymentTerms)
            .NotEmpty().WithMessage("Payment terms are required")
            .MaximumLength(100).WithMessage("Payment terms cannot exceed 100 characters");

        RuleFor(x => x.TaxId)
            .MaximumLength(50).WithMessage("Tax ID cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.TaxId));

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0, 1).WithMessage("Discount percentage must be between 0 and 1 (0% to 100%)");

        RuleFor(x => x.SalesRepresentative)
            .MaximumLength(256).WithMessage("Sales representative cannot exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.SalesRepresentative));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

