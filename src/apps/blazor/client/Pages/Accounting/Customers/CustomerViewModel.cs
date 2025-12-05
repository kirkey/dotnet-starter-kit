namespace FSH.Starter.Blazor.Client.Pages.Accounting.Customers;

/// <summary>
/// ViewModel used by the Customers page for add/edit operations.
/// Mirrors the shape of the API's CreateCustomerCommand and UpdateCustomerCommand so Mapster/Adapt can map between them.
/// </summary>
public class CustomerViewModel
{
    /// <summary>
    /// Primary identifier of the customer.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Unique customer number assigned by the system.
    /// Example: "CUST-2025-001234", "C-10001". Required.
    /// </summary>
    public string CustomerNumber { get; set; } = string.Empty;

    /// <summary>
    /// Customer name (business name or individual name).
    /// Example: "ABC Corporation", "John Smith". Required.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Type of customer for segmentation and reporting.
    /// Values: "Individual", "Business", "Government", "NonProfit", "Wholesale", "Retail".
    /// Default: "Business".
    /// </summary>
    public string CustomerType { get; set; } = "Business";

    /// <summary>
    /// Primary billing address for invoices.
    /// Example: "123 Main St, Suite 100, Anytown, ST 12345". Required.
    /// </summary>
    public string BillingAddress { get; set; } = string.Empty;

    /// <summary>
    /// Shipping address if different from billing address.
    /// Example: "456 Warehouse Rd, Industrial Park, Anytown, ST 12346".
    /// </summary>
    public string? ShippingAddress { get; set; }

    /// <summary>
    /// Primary email address for communications and invoicing.
    /// Example: "billing@abccorp.com".
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Primary phone number.
    /// Example: "(555) 123-4567", "+1-555-123-4567".
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Primary contact person name.
    /// Example: "Jane Doe", "Accounts Payable Manager".
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// Contact person's email address.
    /// Example: "jane.doe@abccorp.com".
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// Contact person's phone number.
    /// Example: "(555) 123-4569".
    /// </summary>
    public string? ContactPhone { get; set; }

    /// <summary>
    /// Credit limit authorized for this customer.
    /// Example: 50000.00 for $50,000 credit line. Default: 0.00 (cash only).
    /// Must be non-negative.
    /// </summary>
    public decimal CreditLimit { get; set; } = 0;

    /// <summary>
    /// Payment terms for this customer.
    /// Example: "Net 30", "Net 15", "2/10 Net 30", "Due on Receipt", "COD".
    /// Default: "Net 30".
    /// </summary>
    public string PaymentTerms { get; set; } = "Net 30";

    /// <summary>
    /// Whether the customer is tax exempt.
    /// Default: false. True requires tax exemption certificate on file.
    /// </summary>
    public bool TaxExempt { get; set; } = false;

    /// <summary>
    /// Tax identification number (EIN, SSN, VAT number).
    /// Example: "12-3456789" for EIN.
    /// Required for tax exempt customers and 1099 reporting.
    /// </summary>
    public string? TaxId { get; set; }

    /// <summary>
    /// Discount percentage applied to invoices.
    /// Example: 5.0 for 5% discount. Default: 0.00.
    /// Must be between 0 and 100.
    /// </summary>
    public decimal DiscountPercentage { get; set; } = 0;

    /// <summary>
    /// Default rate schedule ID for utility billing (if applicable).
    /// </summary>
    public DefaultIdType? DefaultRateScheduleId { get; set; }

    /// <summary>
    /// Accounts receivable account ID for this customer.
    /// </summary>
    public DefaultIdType? ReceivableAccountId { get; set; }

    /// <summary>
    /// Sales representative assigned to this customer.
    /// Example: "John Smith", "Regional Manager - East".
    /// </summary>
    public string? SalesRepresentative { get; set; }

    /// <summary>
    /// Detailed description of the customer's business or relationship.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes or comments about the customer.
    /// Example: special billing instructions, credit terms details, collection notes.
    /// </summary>
    public string? Notes { get; set; }
}

