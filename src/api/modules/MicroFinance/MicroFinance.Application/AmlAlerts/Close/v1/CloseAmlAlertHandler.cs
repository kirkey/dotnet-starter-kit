using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Close.v1;

public sealed class CloseAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IRepository<AmlAlert> repository,
    ILogger<CloseAmlAlertHandler> logger)
    : IRequestHandler<CloseAmlAlertCommand, CloseAmlAlertResponse>
{
    public async Task<CloseAmlAlertResponse> Handle(CloseAmlAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await repository.FirstOrDefaultAsync(new AmlAlertByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"AML alert {request.Id} not found");

        alert.Close();
        await repository.UpdateAsync(alert, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Closed AML alert {Id}", alert.Id);

        return new CloseAmlAlertResponse(alert.Id, alert.Status);
    }
}
