using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.UpgradeTier.v1;

public sealed record UpgradeTierAgentBankingCommand(Guid Id, string NewTier) : IRequest<UpgradeTierAgentBankingResponse>;
