namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Delete.v1;

public sealed record DeletePutAwayTaskCommand : IRequest<DeletePutAwayTaskResponse>
{
    public DefaultIdType PutAwayTaskId { get; set; }
}
