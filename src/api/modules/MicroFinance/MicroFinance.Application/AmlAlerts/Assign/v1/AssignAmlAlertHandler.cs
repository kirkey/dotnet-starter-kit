using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Assign.v1;

public sealed class AssignAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IRepository<AmlAlert> repository,
    ILogger<AssignAmlAlertHandler> logger)
    : IRequestHandler<AssignAmlAlertCommand, AssignAmlAlertResponse>
{
    public async Task<AssignAmlAlertResponse> Handle(AssignAmlAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await repository.FirstOrDefaultAsync(new AmlAlertByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"AML alert {request.Id} not found");

        alert.AssignTo(request.AssignedToId);
        await repository.UpdateAsync(alert, cancellationToken);

        logger.LogInformation("Assigned AML alert {Id} to {AssignedToId}", alert.Id, request.AssignedToId);

        return new AssignAmlAlertResponse(alert.Id, alert.Status, alert.AssignedToId);
    }
}
