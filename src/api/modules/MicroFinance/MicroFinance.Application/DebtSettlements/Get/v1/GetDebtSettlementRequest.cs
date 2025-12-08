using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Get.v1;

public sealed record GetDebtSettlementRequest(DefaultIdType Id) : IRequest<DebtSettlementResponse>;
