using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;

public sealed record GetFeeChargeRequest(DefaultIdType Id) : IRequest<FeeChargeResponse>;
