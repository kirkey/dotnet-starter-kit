namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;

/// <summary>
/// Query to get a put-away task by ID.
/// </summary>
public sealed record GetPutAwayTaskQuery(DefaultIdType PutAwayTaskId) : IRequest<GetPutAwayTaskResponse>;
