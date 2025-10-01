namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Get.v1;

public sealed record GetWholesaleContractQuery(DefaultIdType Id) : IRequest<GetWholesaleContractResponse>;

