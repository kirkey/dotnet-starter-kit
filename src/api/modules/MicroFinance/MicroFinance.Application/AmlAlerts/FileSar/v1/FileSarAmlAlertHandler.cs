using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.FileSar.v1;

public sealed class FileSarAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IRepository<AmlAlert> repository,
    ILogger<FileSarAmlAlertHandler> logger)
    : IRequestHandler<FileSarAmlAlertCommand, FileSarAmlAlertResponse>
{
    public async Task<FileSarAmlAlertResponse> Handle(FileSarAmlAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await repository.FirstOrDefaultAsync(new AmlAlertByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"AML alert {request.Id} not found");

        alert.FileSar(request.SarReference, request.FiledDate);
        await repository.UpdateAsync(alert, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Filed SAR {SarReference} for AML alert {Id}", request.SarReference, alert.Id);

        return new FileSarAmlAlertResponse(alert.Id, alert.Status, alert.SarReference!);
    }
}
