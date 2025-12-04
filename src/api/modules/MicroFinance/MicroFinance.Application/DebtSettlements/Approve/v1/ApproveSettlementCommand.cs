using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Approve.v1;

public sealed record ApproveSettlementCommand(Guid Id, Guid ApprovedById) : IRequest<ApproveSettlementResponse>;
