namespace FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;

public sealed record PickListResponse
{
    public DefaultIdType Id { get; set; }
    public string PickListNumber { get; set; } = default!;
    public DefaultIdType WarehouseId { get; set; }
    public string Status { get; set; } = default!;
    public string PickingType { get; set; } = default!;
    public int Priority { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public int TotalLines { get; set; }
    public int CompletedLines { get; set; }
}
