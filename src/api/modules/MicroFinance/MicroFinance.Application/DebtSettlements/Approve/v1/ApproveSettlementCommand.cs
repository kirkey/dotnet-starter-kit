using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Approve.v1;

public sealed record ApproveSettlementCommand(DefaultIdType Id, DefaultIdType ApprovedById) : IRequest<ApproveSettlementResponse>;
