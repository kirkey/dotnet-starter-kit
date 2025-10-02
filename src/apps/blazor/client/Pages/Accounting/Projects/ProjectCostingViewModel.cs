namespace FSH.Starter.Blazor.Client.Pages.Accounting.Projects;

public class ProjectCostingViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ProjectId { get; set; }
    public DateTime? EntryDate { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public DefaultIdType AccountId { get; set; }
    public string? AccountName { get; set; }
    public DefaultIdType? JournalEntryId { get; set; }
    public string? CostCenter { get; set; }
    public string? WorkOrderNumber { get; set; }
    public bool IsBillable { get; set; }
    public bool IsApproved { get; set; }
    public string? Vendor { get; set; }
    public string? InvoiceNumber { get; set; }
}
