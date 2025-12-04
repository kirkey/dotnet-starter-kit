using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.DebitFloat.v1;

public sealed class DebitFloatHandler(
    ILogger<DebitFloatHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<DebitFloatCommand, DebitFloatResponse>
{
    public async Task<DebitFloatResponse> Handle(DebitFloatCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        agent.DebitFloat(request.Amount);
        await repository.UpdateAsync(agent, cancellationToken);

        logger.LogInformation("Agent {Id} float debited with {Amount}", agent.Id, request.Amount);
        return new DebitFloatResponse(agent.Id, request.Amount, agent.FloatBalance);
    }
}
