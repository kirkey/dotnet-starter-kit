namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;

public sealed class GetPutAwayTaskRequest : IRequest<GetPutAwayTaskResponse>
{
    public DefaultIdType PutAwayTaskId { get; set; }
}
