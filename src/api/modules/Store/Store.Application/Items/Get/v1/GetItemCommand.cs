namespace FSH.Starter.WebApi.Store.Application.Items.Get.v1;

public sealed record GetItemCommand(DefaultIdType Id) : IRequest<ItemResponse>;
