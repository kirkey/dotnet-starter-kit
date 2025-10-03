namespace FSH.Starter.WebApi.Store.Application.PickLists.Delete.v1;

public sealed record DeletePickListCommand : IRequest<DeletePickListResponse>
{
    public DefaultIdType PickListId { get; set; }
}
