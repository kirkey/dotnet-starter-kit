namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Start.v1;

public sealed record StartPutAwayCommand : IRequest<StartPutAwayResponse>
{
    public DefaultIdType PutAwayTaskId { get; set; }
}
