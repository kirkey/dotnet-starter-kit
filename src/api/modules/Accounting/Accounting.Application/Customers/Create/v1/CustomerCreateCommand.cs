namespace Accounting.Application.Customers.Create.v1;

/// <summary>
/// Command to create a new customer account.
/// </summary>
public record CustomerCreateCommand(
    string CustomerNumber,
    string CustomerName,
    string CustomerType,
    string BillingAddress,
    string? ShippingAddress = null,
    string? Email = null,
    string? Phone = null,
    string? ContactName = null,
    decimal CreditLimit = 0,
    string PaymentTerms = "Net 30",
    bool TaxExempt = false,
    string? TaxId = null,
    decimal DiscountPercentage = 0,
    DefaultIdType? DefaultRateScheduleId = null,
    DefaultIdType? ReceivableAccountId = null,
    string? SalesRepresentative = null,
    string? Description = null,
    string? Notes = null
) : IRequest<CustomerCreateResponse>;

