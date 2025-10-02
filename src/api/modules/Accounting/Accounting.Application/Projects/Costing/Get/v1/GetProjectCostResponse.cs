namespace Accounting.Application.Projects.Costing.Get.v1;

/// <summary>
/// Response for the get project cost query containing comprehensive cost entry details.
/// </summary>
/// <param name="ProjectCostId">The unique identifier of the project cost entry</param>
/// <param name="ProjectId">The unique identifier of the associated project</param>
/// <param name="EntryDate">The date when the cost was incurred</param>
/// <param name="Amount">The cost amount</param>
/// <param name="Description">Description of the cost entry</param>
/// <param name="Category">The cost category</param>
/// <param name="AccountId">Reference to the chart of accounts entry</param>
/// <param name="JournalEntryId">Optional reference to journal entry</param>
/// <param name="CostCenter">Optional cost center for departmental allocation</param>
/// <param name="WorkOrderNumber">Optional work order number reference</param>
/// <param name="IsBillable">Whether this cost can be billed to client</param>
/// <param name="IsApproved">Whether this cost entry has been approved</param>
/// <param name="Vendor">Optional vendor or supplier reference</param>
/// <param name="InvoiceNumber">Optional invoice or receipt number</param>
/// <param name="CreatedOn">When the cost entry was created (UTC)</param>
/// <param name="LastModifiedOn">When the cost entry was last modified (UTC)</param>
public sealed record GetProjectCostResponse(
    DefaultIdType ProjectCostId,
    DefaultIdType ProjectId,
    DateTime EntryDate,
    decimal Amount,
    string Description,
    string? Category,
    DefaultIdType AccountId,
    DefaultIdType? JournalEntryId,
    string? CostCenter,
    string? WorkOrderNumber,
    bool IsBillable,
    bool IsApproved,
    string? Vendor,
    string? InvoiceNumber,
    DateTimeOffset CreatedOn,
    DateTimeOffset? LastModifiedOn);
