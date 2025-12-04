using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Approve.v1;

public sealed record ApproveAgentBankingCommand(Guid Id) : IRequest<ApproveAgentBankingResponse>;
