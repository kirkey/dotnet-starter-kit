namespace Accounting.Application.Invoices.Update.v1;

/// <summary>
/// Command to update an existing invoice.
/// Only updates provided fields; null values are ignored.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
/// <param name="DueDate">Updated due date.</param>
/// <param name="UsageCharge">Updated usage-based charge.</param>
/// <param name="BasicServiceCharge">Updated fixed monthly charge.</param>
/// <param name="TaxAmount">Updated tax amount.</param>
/// <param name="OtherCharges">Updated other charges.</param>
/// <param name="LateFee">Updated late fee.</param>
/// <param name="ReconnectionFee">Updated reconnection fee.</param>
/// <param name="DepositAmount">Updated deposit amount.</param>
/// <param name="DemandCharge">Updated demand charge.</param>
/// <param name="RateSchedule">Updated rate schedule.</param>
/// <param name="Description">Updated description.</param>
/// <param name="Notes">Updated notes.</param>
public sealed record UpdateInvoiceCommand(
    DefaultIdType InvoiceId,
    DateTime? DueDate = null,
    decimal? UsageCharge = null,
    decimal? BasicServiceCharge = null,
    decimal? TaxAmount = null,
    decimal? OtherCharges = null,
    decimal? LateFee = null,
    decimal? ReconnectionFee = null,
    decimal? DepositAmount = null,
    decimal? DemandCharge = null,
    string? RateSchedule = null,
    string? Description = null,
    string? Notes = null
) : IRequest<UpdateInvoiceResponse>;

