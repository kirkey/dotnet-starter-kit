using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Approve.v1;

public sealed class ApproveAgentBankingHandler(
    ILogger<ApproveAgentBankingHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<ApproveAgentBankingCommand, ApproveAgentBankingResponse>
{
    public async Task<ApproveAgentBankingResponse> Handle(ApproveAgentBankingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        agent.Approve();
        await repository.UpdateAsync(agent, cancellationToken);

        logger.LogInformation("Agent banking {Id} approved", agent.Id);
        return new ApproveAgentBankingResponse(agent.Id, agent.Status);
    }
}
