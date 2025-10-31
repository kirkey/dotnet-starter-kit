namespace Accounting.Application.WriteOffs.Create.v1;

/// <summary>
/// Command to create a new write-off.
/// </summary>
public record WriteOffCreateCommand(
    string ReferenceNumber,
    DateTime WriteOffDate,
    string WriteOffType,
    decimal Amount,
    DefaultIdType ReceivableAccountId,
    DefaultIdType ExpenseAccountId,
    DefaultIdType? CustomerId = null,
    string? CustomerName = null,
    DefaultIdType? InvoiceId = null,
    string? InvoiceNumber = null,
    string? Reason = null,
    string? Description = null,
    string? Notes = null
) : IRequest<WriteOffCreateResponse>;

