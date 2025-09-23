using System.ComponentModel;

namespace Accounting.Application.ProjectCosting.Create.v1;

/// <summary>
/// Command to create a new project cost entry with comprehensive validation.
/// </summary>
/// <param name="ProjectId">The unique identifier of the associated project</param>
/// <param name="EntryDate">The date when the cost was incurred</param>
/// <param name="Amount">The cost amount (must be positive)</param>
/// <param name="Description">Description of the cost entry</param>
/// <param name="AccountId">Reference to the chart of accounts entry</param>
/// <param name="Category">Optional cost category for classification</param>
/// <param name="JournalEntryId">Optional reference to journal entry</param>
/// <param name="CostCenter">Optional cost center for departmental allocation</param>
/// <param name="WorkOrderNumber">Optional work order number reference</param>
/// <param name="IsBillable">Whether this cost can be billed to client</param>
/// <param name="Vendor">Optional vendor or supplier reference</param>
/// <param name="InvoiceNumber">Optional invoice or receipt number</param>
public sealed record CreateProjectCostCommand(
    DefaultIdType ProjectId,
    [property: DefaultValue("2025-09-23")] DateTime EntryDate,
    [property: DefaultValue(1250.75)] decimal Amount,
    [property: DefaultValue("Materials for electrical work")] string Description,
    DefaultIdType AccountId,
    [property: DefaultValue("Materials")] string? Category = null,
    [property: DefaultValue(null)] DefaultIdType? JournalEntryId = null,
    [property: DefaultValue("CC001")] string? CostCenter = null,
    [property: DefaultValue("WO-2025-001")] string? WorkOrderNumber = null,
    [property: DefaultValue(false)] bool IsBillable = false,
    [property: DefaultValue("ABC Supply Co")] string? Vendor = null,
    [property: DefaultValue("INV-12345")] string? InvoiceNumber = null) : IRequest<CreateProjectCostResponse>;
