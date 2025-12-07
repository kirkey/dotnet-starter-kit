using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Escalate.v1;

public sealed class EscalateAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IRepository<AmlAlert> repository,
    ILogger<EscalateAmlAlertHandler> logger)
    : IRequestHandler<EscalateAmlAlertCommand, EscalateAmlAlertResponse>
{
    public async Task<EscalateAmlAlertResponse> Handle(EscalateAmlAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await repository.FirstOrDefaultAsync(new AmlAlertByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"AML alert {request.Id} not found");

        alert.Escalate(request.Reason);
        await repository.UpdateAsync(alert, cancellationToken);

        logger.LogInformation("Escalated AML alert {Id}", alert.Id);

        return new EscalateAmlAlertResponse(alert.Id, alert.Status);
    }
}
