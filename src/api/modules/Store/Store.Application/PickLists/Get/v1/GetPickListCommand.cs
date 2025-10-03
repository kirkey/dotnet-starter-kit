namespace FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;

public sealed record GetPickListCommand : IRequest<GetPickListResponse>
{
    public DefaultIdType PickListId { get; set; }
}
