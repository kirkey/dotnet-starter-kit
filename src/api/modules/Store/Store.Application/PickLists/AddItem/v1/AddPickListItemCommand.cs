namespace FSH.Starter.WebApi.Store.Application.PickLists.AddItem.v1;

public sealed record AddPickListItemCommand : IRequest<AddPickListItemResponse>
{
    public DefaultIdType PickListId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DefaultIdType? SerialNumberId { get; set; }
    public int QuantityToPick { get; set; }
    public string? Notes { get; set; }
}
