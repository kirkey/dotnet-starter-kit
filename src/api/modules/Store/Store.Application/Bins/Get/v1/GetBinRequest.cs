namespace FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

public sealed record GetBinRequest(DefaultIdType Id) : IRequest<BinResponse>;
