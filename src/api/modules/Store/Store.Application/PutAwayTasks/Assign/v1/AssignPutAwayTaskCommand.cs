namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Assign.v1;

public sealed record AssignPutAwayTaskCommand : IRequest<AssignPutAwayTaskResponse>
{
    public DefaultIdType PutAwayTaskId { get; set; }
    public string AssignedTo { get; set; } = default!;
}
