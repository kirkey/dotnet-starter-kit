namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

/// <summary>
/// Response for add put-away task item operation.
/// </summary>
/// <remarks>
/// Contains the identifier of the newly added put-away task item.
/// Used to return the result of put-away task item addition operations.
/// </remarks>
public sealed record AddPutAwayTaskItemResponse(DefaultIdType Id);
