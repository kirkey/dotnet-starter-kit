namespace Accounting.Application.Projects.Costing.Get;

/// <summary>
/// Response DTO representing a project cost entry with key accounting details.
/// </summary>
/// <param name="Id">Unique identifier of the project cost entry.</param>
/// <param name="ProjectId">Identifier of the project this entry belongs to.</param>
/// <param name="EntryDate">The date when the cost was incurred.</param>
/// <param name="Description">Human-friendly description of the cost.</param>
/// <param name="Amount">Positive amount of the cost entry.</param>
/// <param name="AccountId">Chart of Accounts identifier associated with this cost.</param>
/// <param name="JournalEntryId">Optional Journal Entry reference if posted.</param>
/// <param name="Category">Optional category for reporting.
/// Common examples: Materials, Labor, Services, Transportation.</param>
/// <param name="CostCenter">Optional cost center for allocations.</param>
/// <param name="WorkOrderNumber">Optional work order reference.</param>
/// <param name="IsBillable">Whether this cost is billable to a client or project sponsor.</param>
/// <param name="IsApproved">Whether this cost has passed the approval workflow.</param>
/// <param name="Vendor">Optional vendor/supplier for the cost.</param>
/// <param name="InvoiceNumber">Optional source document reference like an invoice or receipt number.</param>
public sealed record ProjectCostResponse(
    DefaultIdType Id,
    DefaultIdType ProjectId,
    DateTime EntryDate,
    string Description,
    decimal Amount,
    DefaultIdType AccountId,
    DefaultIdType? JournalEntryId,
    string? Category,
    string? CostCenter,
    string? WorkOrderNumber,
    bool IsBillable,
    bool IsApproved,
    string? Vendor,
    string? InvoiceNumber);

