namespace FSH.Starter.WebApi.Store.Application.PickLists.Update.v1;

/// <summary>
/// Command to update an existing pick list.
/// </summary>
public record UpdatePickListCommand : IRequest<UpdatePickListResponse>
{
    /// <summary>
    /// Gets or sets the pick list identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the pick list name.
    /// </summary>
    [DefaultValue("Pick List PICK-2025-001")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the pick list description.
    /// </summary>
    [DefaultValue("Pick list for order fulfillment")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the pick list number.
    /// </summary>
    [DefaultValue("PICK-2025-001")]
    public string PickListNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    public DefaultIdType WarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the status (Created, Assigned, InProgress, Completed, Cancelled).
    /// </summary>
    [DefaultValue("Created")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the picking type (Order, Wave, Batch, Zone, Standard).
    /// </summary>
    [DefaultValue("Standard")]
    public string PickingType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    [DefaultValue(0)]
    public int Priority { get; set; }

    /// <summary>
    /// Gets or sets the assigned picker.
    /// </summary>
    [DefaultValue(null)]
    public string? AssignedTo { get; set; }

    /// <summary>
    /// Gets or sets the start date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the completed date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// Gets or sets the expected completion date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ExpectedCompletionDate { get; set; }

    /// <summary>
    /// Gets or sets the reference number.
    /// </summary>
    [DefaultValue(null)]
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}
