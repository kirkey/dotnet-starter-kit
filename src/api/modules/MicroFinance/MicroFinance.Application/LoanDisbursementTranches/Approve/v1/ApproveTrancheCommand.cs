using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Approve.v1;

public sealed record ApproveTrancheCommand(DefaultIdType Id, DefaultIdType UserId) : IRequest<ApproveTrancheResponse>;
