namespace Accounting.Application.Payments.Update.v1;

/// <summary>
/// Command to update an existing payment.
/// </summary>
/// <remarks>
/// Only updates basic payment information.
/// Cannot update Amount or Allocations through this command.
/// Use AllocatePayment, RefundPayment, or VoidPayment for those operations.
/// </remarks>
public sealed record PaymentUpdateCommand : IRequest<PaymentUpdateResponse>
{
    /// <summary>
    /// Payment identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Reference number.
    /// </summary>
    public string? ReferenceNumber { get; init; }
    
    /// <summary>
    /// Deposit to account code.
    /// </summary>
    public string? DepositToAccountCode { get; init; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; init; }
}
