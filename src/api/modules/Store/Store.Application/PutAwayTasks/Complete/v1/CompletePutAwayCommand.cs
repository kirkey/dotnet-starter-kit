namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Complete.v1;

public sealed record CompletePutAwayCommand : IRequest<CompletePutAwayResponse>
{
    public DefaultIdType PutAwayTaskId { get; set; }
}
