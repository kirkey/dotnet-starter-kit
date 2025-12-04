using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Get.v1;

/// <summary>
/// Handler for getting a promise to pay by ID.
/// </summary>
public sealed class GetPromiseToPayHandler(
    [FromKeyedServices("microfinance:promisetopays")] IReadRepository<PromiseToPay> repository,
    ILogger<GetPromiseToPayHandler> logger)
    : IRequestHandler<GetPromiseToPayRequest, PromiseToPayResponse>
{
    public async Task<PromiseToPayResponse> Handle(GetPromiseToPayRequest request, CancellationToken cancellationToken)
    {
        var spec = new PromiseToPayByIdSpec(request.Id);
        var promiseToPay = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (promiseToPay is null)
        {
            throw new NotFoundException($"Promise to pay with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved promise to pay {PromiseToPayId}", request.Id);

        return promiseToPay;
    }
}
