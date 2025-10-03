namespace FSH.Starter.WebApi.Store.Application.PickLists.Assign.v1;

public sealed record AssignPickListCommand : IRequest<AssignPickListResponse>
{
    public DefaultIdType PickListId { get; set; }
    public string AssignedTo { get; set; } = default!;
}
