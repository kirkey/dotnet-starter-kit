namespace Accounting.Application.Invoices.Update.v1;

/// <summary>
/// Command to update an existing invoice.
/// Only updates provided fields; null values are ignored.
/// </summary>
public sealed record UpdateInvoiceCommand : IRequest<UpdateInvoiceResponse>
{
    /// <summary>
    /// Invoice identifier.
    /// </summary>
    public DefaultIdType InvoiceId { get; init; }
    
    /// <summary>
    /// Updated due date.
    /// </summary>
    public DateTime? DueDate { get; init; }
    
    /// <summary>
    /// Updated usage-based charge.
    /// </summary>
    public decimal? UsageCharge { get; init; }
    
    /// <summary>
    /// Updated fixed monthly charge.
    /// </summary>
    public decimal? BasicServiceCharge { get; init; }
    
    /// <summary>
    /// Updated tax amount.
    /// </summary>
    public decimal? TaxAmount { get; init; }
    
    /// <summary>
    /// Updated other charges.
    /// </summary>
    public decimal? OtherCharges { get; init; }
    
    /// <summary>
    /// Updated late fee.
    /// </summary>
    public decimal? LateFee { get; init; }
    
    /// <summary>
    /// Updated reconnection fee.
    /// </summary>
    public decimal? ReconnectionFee { get; init; }
    
    /// <summary>
    /// Updated deposit amount.
    /// </summary>
    public decimal? DepositAmount { get; init; }
    
    /// <summary>
    /// Updated demand charge.
    /// </summary>
    public decimal? DemandCharge { get; init; }
    
    /// <summary>
    /// Updated rate schedule.
    /// </summary>
    public string? RateSchedule { get; init; }
    
    /// <summary>
    /// Updated description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Updated notes.
    /// </summary>
    public string? Notes { get; init; }
}
