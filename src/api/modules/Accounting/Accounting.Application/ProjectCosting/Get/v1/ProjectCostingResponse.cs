namespace Accounting.Application.ProjectCosting.Get.v1;

/// <summary>
/// Response containing project costing entry details.
/// </summary>
public sealed record ProjectCostingResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType ProjectId { get; init; }
    public DateTime EntryDate { get; init; }
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public DefaultIdType AccountId { get; init; }
    public DefaultIdType? JournalEntryId { get; init; }
    public string? CostCenter { get; init; }
    public string? WorkOrderNumber { get; init; }
    public bool IsBillable { get; init; }
    public bool IsApproved { get; init; }
    public string? Vendor { get; init; }
    public string? InvoiceNumber { get; init; }
    public DateTime? CreatedOn { get; init; }
    public DefaultIdType? CreatedBy { get; init; }
    public DateTime? LastModifiedOn { get; init; }
    public DefaultIdType? LastModifiedBy { get; init; }
}
