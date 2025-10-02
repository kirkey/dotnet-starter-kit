namespace Accounting.Application.Projects.Costing.Create;

/// <summary>
/// Command to create a new project costing entry.
/// </summary>
/// <param name="ProjectId">The unique identifier of the associated project.</param>
/// <param name="EntryDate">The date when the cost was incurred.</param>
/// <param name="Amount">The cost amount (must be positive).</param>
/// <param name="Description">Description of the cost entry.</param>
/// <param name="AccountId">Reference to the chart of accounts entry.</param>
/// <param name="Category">Optional cost category for classification.</param>
/// <param name="JournalEntryId">Optional reference to journal entry.</param>
/// <param name="CostCenter">Optional cost center for departmental allocation.</param>
/// <param name="WorkOrderNumber">Optional work order number reference.</param>
/// <param name="IsBillable">Whether this cost can be billed to client.</param>
/// <param name="Vendor">Optional vendor or supplier reference.</param>
/// <param name="InvoiceNumber">Optional invoice or receipt number.</param>
public sealed record CreateProjectCostingCommand(
    DefaultIdType ProjectId,
    DateTime EntryDate,
    decimal Amount,
    string Description,
    DefaultIdType AccountId,
    string? Category = null,
    DefaultIdType? JournalEntryId = null,
    string? CostCenter = null,
    string? WorkOrderNumber = null,
    bool IsBillable = false,
    string? Vendor = null,
    string? InvoiceNumber = null
) : IRequest<DefaultIdType>;
