using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;

public sealed record GetFixedDepositRequest(DefaultIdType Id) : IRequest<FixedDepositResponse>;
