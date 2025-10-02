namespace Accounting.Application.Projects.Costing.Responses;

/// <summary>
/// Detailed response for a ProjectCostEntry.
/// </summary>
public sealed record ProjectCostingResponse(
    DefaultIdType Id,
    DefaultIdType ProjectId,
    DateTime EntryDate,
    decimal Amount,
    string? Description,
    DefaultIdType AccountId,
    string? Category,
    DefaultIdType? JournalEntryId,
    string? CostCenter,
    string? WorkOrderNumber,
    bool IsBillable,
    bool IsApproved,
    string? Vendor,
    string? InvoiceNumber
);
