using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.CreditFloat.v1;

public sealed class CreditFloatHandler(
    ILogger<CreditFloatHandler> logger,
    [FromKeyedServices("microfinance:agentbankings")] IRepository<AgentBanking> repository)
    : IRequestHandler<CreditFloatCommand, CreditFloatResponse>
{
    public async Task<CreditFloatResponse> Handle(CreditFloatCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var agent = await repository.FirstOrDefaultAsync(new AgentBankingByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Agent banking with id {request.Id} not found");

        agent.CreditFloat(request.Amount);
        await repository.UpdateAsync(agent, cancellationToken);

        logger.LogInformation("Agent {Id} float credited with {Amount}", agent.Id, request.Amount);
        return new CreditFloatResponse(agent.Id, request.Amount, agent.FloatBalance);
    }
}
