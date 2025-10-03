namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

/// <summary>
/// Command to add an item to a put-away task.
/// </summary>
public sealed record AddPutAwayTaskItemCommand(
    DefaultIdType PutAwayTaskId,
    DefaultIdType ItemId,
    DefaultIdType ToBinId,
    DefaultIdType? LotNumberId,
    DefaultIdType? SerialNumberId,
    int Quantity,
    string? Notes
) : IRequest<AddPutAwayTaskItemResponse>;
