namespace FSH.Starter.WebApi.Store.Application.PickLists.Start.v1;

public sealed record StartPickingCommand : IRequest<StartPickingResponse>
{
    public DefaultIdType PickListId { get; set; }
}
