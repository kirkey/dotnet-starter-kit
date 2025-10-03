namespace FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;

/// <summary>
/// Command to get a pick list by ID.
/// </summary>
public sealed record GetPickListCommand(DefaultIdType PickListId) : IRequest<GetPickListResponse>;
