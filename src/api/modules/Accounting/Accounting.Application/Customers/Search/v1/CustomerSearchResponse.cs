namespace Accounting.Application.Customers.Search.v1;

/// <summary>
/// Response DTO for customer search results.
/// Contains essential customer information for list/grid display.
/// </summary>
public record CustomerSearchResponse
{
    /// <summary>
    /// Unique identifier of the customer.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Unique customer number.
    /// Example: "CUST-2025-001234", "C-10001".
    /// </summary>
    public string CustomerNumber { get; init; } = string.Empty;

    /// <summary>
    /// Customer name (business name or individual name).
    /// Example: "ABC Corporation", "John Smith".
    /// </summary>
    public string CustomerName { get; init; } = string.Empty;

    /// <summary>
    /// Type of customer for segmentation.
    /// Values: "Individual", "Business", "Government", "NonProfit", "Wholesale", "Retail".
    /// </summary>
    public string CustomerType { get; init; } = string.Empty;

    /// <summary>
    /// Primary email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Primary phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Primary billing address.
    /// </summary>
    public string? BillingAddress { get; init; }

    /// <summary>
    /// Credit limit authorized for this customer.
    /// </summary>
    public decimal CreditLimit { get; init; }

    /// <summary>
    /// Current account balance owed by customer.
    /// </summary>
    public decimal CurrentBalance { get; init; }

    /// <summary>
    /// Available credit (CreditLimit - CurrentBalance).
    /// </summary>
    public decimal AvailableCredit { get; init; }

    /// <summary>
    /// Account status.
    /// Values: "Active", "Inactive", "CreditHold", "PastDue", "Collections", "Closed".
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Whether the customer is tax exempt.
    /// </summary>
    public bool TaxExempt { get; init; }

    /// <summary>
    /// Payment terms for this customer.
    /// Example: "Net 30", "Net 15", "2/10 Net 30".
    /// </summary>
    public string? PaymentTerms { get; init; }

    /// <summary>
    /// Whether the customer account is active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Whether the customer is on credit hold.
    /// </summary>
    public bool IsOnCreditHold { get; init; }

    /// <summary>
    /// Description of the customer's business.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Additional notes about the customer.
    /// </summary>
    public string? Notes { get; init; }
}

