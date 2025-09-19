namespace Accounting.Application.Projects.Responses;

/// <summary>
/// Response model representing a job costing entry for a project.
/// Contains cost tracking information for project expenses and time entries.
/// </summary>
public class JobCostingEntryResponse(
    DefaultIdType projectId,
    DateTime entryDate,
    decimal amount,
    string costType,
    string? reference) : BaseDto
{
    /// <summary>
    /// Reference to the project this costing entry belongs to.
    /// </summary>
    public DefaultIdType ProjectId { get; set; } = projectId;
    
    /// <summary>
    /// Date when this cost was incurred or recorded.
    /// </summary>
    public DateTime EntryDate { get; set; } = entryDate;
    
    /// <summary>
    /// Amount of the cost entry.
    /// </summary>
    public decimal Amount { get; set; } = amount;
    
    /// <summary>
    /// Type or category of cost (e.g., "Labor", "Materials", "Equipment").
    /// </summary>
    public string CostType { get; set; } = costType;
    
    /// <summary>
    /// Optional reference number or document reference for this cost entry.
    /// </summary>
    public string? Reference { get; set; } = reference;
}
