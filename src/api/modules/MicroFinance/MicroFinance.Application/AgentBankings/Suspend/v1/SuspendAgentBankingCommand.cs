using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Suspend.v1;

public sealed record SuspendAgentBankingCommand(Guid Id, string Reason) : IRequest<SuspendAgentBankingResponse>;
