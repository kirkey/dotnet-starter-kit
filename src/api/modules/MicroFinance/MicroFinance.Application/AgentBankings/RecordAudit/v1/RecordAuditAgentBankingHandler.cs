using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.RecordAudit.v1;

public sealed class RecordAuditAgentBankingHandler(
    ILogger<RecordAuditAgentBankingHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<RecordAuditAgentBankingCommand, RecordAuditAgentBankingResponse>
{
    public async Task<RecordAuditAgentBankingResponse> Handle(RecordAuditAgentBankingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        agent.RecordAudit(request.AuditDate);
        await repository.UpdateAsync(agent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Agent banking {Id} audit recorded for {AuditDate}", agent.Id, request.AuditDate);
        return new RecordAuditAgentBankingResponse(agent.Id, agent.LastAuditDate!.Value);
    }
}
