using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Confirm.v1;

public sealed class ConfirmAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IRepository<AmlAlert> repository,
    ILogger<ConfirmAmlAlertHandler> logger)
    : IRequestHandler<ConfirmAmlAlertCommand, ConfirmAmlAlertResponse>
{
    public async Task<ConfirmAmlAlertResponse> Handle(ConfirmAmlAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await repository.FirstOrDefaultAsync(new AmlAlertByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"AML alert {request.Id} not found");

        alert.ConfirmSuspicious(request.ResolvedById, request.Notes);
        await repository.UpdateAsync(alert, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Confirmed suspicious activity for AML alert {Id}", alert.Id);

        return new ConfirmAmlAlertResponse(alert.Id, alert.Status);
    }
}
