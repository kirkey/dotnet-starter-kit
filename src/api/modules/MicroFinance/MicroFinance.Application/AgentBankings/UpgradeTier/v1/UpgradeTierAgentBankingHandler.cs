using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.UpgradeTier.v1;

public sealed class UpgradeTierAgentBankingHandler(
    ILogger<UpgradeTierAgentBankingHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<UpgradeTierAgentBankingCommand, UpgradeTierAgentBankingResponse>
{
    public async Task<UpgradeTierAgentBankingResponse> Handle(UpgradeTierAgentBankingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        agent.UpgradeTier(request.NewTier);
        await repository.UpdateAsync(agent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Agent banking {Id} tier upgraded to {Tier}", agent.Id, agent.Tier);
        return new UpgradeTierAgentBankingResponse(agent.Id, agent.Tier);
    }
}
