namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

/// <summary>
/// Command to add an item to a put-away task.
/// </summary>
public sealed record AddPutAwayTaskItemCommand : IRequest<AddPutAwayTaskItemResponse>
{
    public DefaultIdType PutAwayTaskId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType ToBinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public DefaultIdType? SerialNumberId { get; set; }
    public int QuantityToPutAway { get; set; }
    public int SequenceNumber { get; set; }
    public string? Notes { get; set; }
}
