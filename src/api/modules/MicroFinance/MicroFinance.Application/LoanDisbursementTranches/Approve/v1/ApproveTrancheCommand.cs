using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Approve.v1;

public sealed record ApproveTrancheCommand(Guid Id, Guid UserId) : IRequest<ApproveTrancheResponse>;
