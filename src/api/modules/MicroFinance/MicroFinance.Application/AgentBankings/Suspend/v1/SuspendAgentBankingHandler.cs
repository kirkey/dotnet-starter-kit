using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Suspend.v1;

public sealed class SuspendAgentBankingHandler(
    ILogger<SuspendAgentBankingHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<SuspendAgentBankingCommand, SuspendAgentBankingResponse>
{
    public async Task<SuspendAgentBankingResponse> Handle(SuspendAgentBankingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        agent.Suspend(request.Reason);
        await repository.UpdateAsync(agent, cancellationToken);

        logger.LogInformation("Agent banking {Id} suspended", agent.Id);
        return new SuspendAgentBankingResponse(agent.Id, agent.Status);
    }
}
