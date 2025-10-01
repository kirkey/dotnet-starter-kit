namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Get.v1;

public sealed record GetWholesalePricingQuery(DefaultIdType Id) : IRequest<GetWholesalePricingResponse>;

