namespace FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;

/// <summary>
/// Response model for getting a pick list with all its details.
/// </summary>
public sealed record GetPickListResponse
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string PickListNumber { get; set; } = default!;
    public DefaultIdType WarehouseId { get; set; }
    public string WarehouseName { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string PickingType { get; set; } = default!;
    public int Priority { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? ExpectedCompletionDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
    public int TotalLines { get; set; }
    public int PickedLines { get; set; }
    public decimal CompletionPercentage { get; set; }
    public IReadOnlyCollection<PickListItemDto> Items { get; init; } = [];
}

/// <summary>
/// Data transfer object for pick list item details.
/// </summary>
public sealed record PickListItemDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string ItemName { get; set; } = default!;
    public DefaultIdType? BinId { get; set; }
    public string? BinName { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DefaultIdType? SerialNumberId { get; set; }
    public int QuantityToPick { get; set; }
    public int QuantityPicked { get; set; }
    public string Status { get; set; } = default!;
    public int SequenceNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime? PickedDate { get; set; }
}
