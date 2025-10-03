namespace FSH.Starter.WebApi.Store.Application.PickLists.Complete.v1;

public sealed record CompletePickingCommand : IRequest<CompletePickingResponse>
{
    public DefaultIdType PickListId { get; set; }
}
