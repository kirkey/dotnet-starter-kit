namespace FSH.Starter.WebApi.Store.Application.PickLists.Create.v1;

public sealed record CreatePickListCommand : IRequest<CreatePickListResponse>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string PickListNumber { get; set; } = default!;
    public DefaultIdType WarehouseId { get; set; }
    public string PickingType { get; set; } = default!;
    public int Priority { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
}
