using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Delete.v1;

public sealed class DeletePayComponentRateHandler(
    ILogger<DeletePayComponentRateHandler> logger,
    [FromKeyedServices("humanresources:paycomponentrates")] IRepository<PayComponentRate> repository)
    : IRequestHandler<DeletePayComponentRateCommand, DeletePayComponentRateResponse>
{
    public async Task<DeletePayComponentRateResponse> Handle(DeletePayComponentRateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rate = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = rate ?? throw new PayComponentRateNotFoundException(request.Id);

        await repository.DeleteAsync(rate, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Pay component rate with id {RateId} deleted.", rate.Id);

        return new DeletePayComponentRateResponse(rate.Id);
    }
}

