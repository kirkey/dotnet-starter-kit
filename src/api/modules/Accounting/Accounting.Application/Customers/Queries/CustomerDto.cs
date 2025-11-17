namespace Accounting.Application.Customers.Queries;

/// <summary>
/// Customer data transfer object for list views.
/// </summary>
public record CustomerDto
{
    public DefaultIdType Id { get; init; }
    public string CustomerNumber { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string CustomerType { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public decimal CreditLimit { get; init; }
    public decimal CurrentBalance { get; init; }
    public decimal AvailableCredit { get; init; }
    public bool IsActive { get; init; }
    public bool IsOnCreditHold { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? ImageUrl { get; init; }
}

/// <summary>
/// Customer data transfer object for detail view with all properties.
/// </summary>
public record CustomerDetailsDto : CustomerDto
{
    public string BillingAddress { get; init; } = string.Empty;
    public string? ShippingAddress { get; init; }
    public string? Fax { get; init; }
    public string? ContactName { get; init; }
    public string? ContactEmail { get; init; }
    public string? ContactPhone { get; init; }
    public string PaymentTerms { get; init; } = string.Empty;
    public bool TaxExempt { get; init; }
    public string? TaxId { get; init; }
    public decimal DiscountPercentage { get; init; }
    public DateTimeOffset AccountOpenDate { get; init; }
    public DateTime? LastTransactionDate { get; init; }
    public DateTime? LastPaymentDate { get; init; }
    public decimal? LastPaymentAmount { get; init; }
    public DefaultIdType? DefaultRateScheduleId { get; init; }
    public DefaultIdType? ReceivableAccountId { get; init; }
    public string? SalesRepresentative { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

