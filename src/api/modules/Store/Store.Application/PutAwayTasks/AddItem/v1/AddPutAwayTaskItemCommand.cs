namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

public sealed class AddPutAwayTaskItemCommand : IRequest<AddPutAwayTaskItemResponse>
{
    public DefaultIdType PutAwayTaskId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType ToBinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DefaultIdType? SerialNumberId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
