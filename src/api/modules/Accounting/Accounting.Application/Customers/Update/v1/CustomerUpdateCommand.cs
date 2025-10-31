namespace Accounting.Application.Customers.Update.v1;

/// <summary>
/// Command to update an existing customer.
/// </summary>
public record CustomerUpdateCommand(
    DefaultIdType Id,
    string? CustomerName = null,
    string? BillingAddress = null,
    string? ShippingAddress = null,
    string? Email = null,
    string? Phone = null,
    string? ContactName = null,
    string? ContactEmail = null,
    string? ContactPhone = null,
    string? PaymentTerms = null,
    bool? TaxExempt = null,
    string? TaxId = null,
    decimal? DiscountPercentage = null,
    string? SalesRepresentative = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

