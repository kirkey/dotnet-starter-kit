namespace Accounting.Application.PaymentAllocations.Responses;

/// <summary>
/// Response model representing a payment allocation entry.
/// Contains information about how payments are allocated to specific invoices.
/// </summary>
public class PaymentAllocationResponse
{
    /// <summary>
    /// Unique identifier for the payment allocation.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Reference to the payment being allocated.
    /// </summary>
    public DefaultIdType PaymentId { get; set; }
    
    /// <summary>
    /// Reference to the invoice receiving the allocation.
    /// </summary>
    public DefaultIdType InvoiceId { get; set; }
    
    /// <summary>
    /// Amount of the payment allocated to this invoice.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Optional notes or comments about this payment allocation.
    /// </summary>
    public string? Notes { get; set; }
}
