namespace FSH.Starter.WebApi.Store.Application.PriceLists.Get.v1;

public sealed record GetPriceListQuery(DefaultIdType Id) : IRequest<GetPriceListResponse>;

