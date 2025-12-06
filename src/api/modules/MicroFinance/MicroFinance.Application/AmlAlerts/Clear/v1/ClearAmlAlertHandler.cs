using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Clear.v1;

public sealed class ClearAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IRepository<AmlAlert> repository,
    ILogger<ClearAmlAlertHandler> logger)
    : IRequestHandler<ClearAmlAlertCommand, ClearAmlAlertResponse>
{
    public async Task<ClearAmlAlertResponse> Handle(ClearAmlAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await repository.FirstOrDefaultAsync(new AmlAlertByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new Exception($"AML alert {request.Id} not found");

        alert.Clear(request.ResolvedById, request.Notes);
        await repository.UpdateAsync(alert, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Cleared AML alert {Id} as non-suspicious", alert.Id);

        return new ClearAmlAlertResponse(alert.Id, alert.Status);
    }
}
