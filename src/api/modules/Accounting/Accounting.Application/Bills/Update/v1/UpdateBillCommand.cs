namespace Accounting.Application.Bills.Update.v1;

/// <summary>
/// Command to update an existing bill.
/// </summary>
public sealed record UpdateBillCommand : IRequest<UpdateBillResponse>
{
    /// <summary>
    /// Bill identifier.
    /// </summary>
    public DefaultIdType BillId { get; init; }
    
    /// <summary>
    /// Bill or vendor invoice number.
    /// </summary>
    public string? BillNumber { get; init; }
    
    /// <summary>
    /// Date bill was issued.
    /// </summary>
    public DateTime? BillDate { get; init; }
    
    /// <summary>
    /// Date payment is due.
    /// </summary>
    public DateTime? DueDate { get; init; }
    
    /// <summary>
    /// Description of the bill.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Optional accounting period.
    /// </summary>
    public DefaultIdType? PeriodId { get; init; }
    
    /// <summary>
    /// Optional payment terms.
    /// </summary>
    public string? PaymentTerms { get; init; }
    
    /// <summary>
    /// Optional PO reference.
    /// </summary>
    public string? PurchaseOrderNumber { get; init; }
    
    /// <summary>
    /// Optional notes.
    /// </summary>
    public string? Notes { get; init; }
}
