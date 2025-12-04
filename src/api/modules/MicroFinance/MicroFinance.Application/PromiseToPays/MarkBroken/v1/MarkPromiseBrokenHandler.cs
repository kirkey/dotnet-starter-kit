using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.MarkBroken.v1;

/// <summary>
/// Handler for marking a promise to pay as broken.
/// </summary>
public sealed class MarkPromiseBrokenHandler(
    [FromKeyedServices("microfinance:promisetopays")] IRepository<PromiseToPay> repository,
    ILogger<MarkPromiseBrokenHandler> logger)
    : IRequestHandler<MarkPromiseBrokenCommand, MarkPromiseBrokenResponse>
{
    public async Task<MarkPromiseBrokenResponse> Handle(MarkPromiseBrokenCommand request, CancellationToken cancellationToken)
    {
        var promiseToPay = await repository.GetByIdAsync(request.PromiseId, cancellationToken);

        if (promiseToPay is null)
        {
            throw new NotFoundException($"Promise to pay with ID {request.PromiseId} not found.");
        }

        promiseToPay.MarkAsBroken(request.Reason);
        await repository.UpdateAsync(promiseToPay, cancellationToken);

        logger.LogInformation("Promise {PromiseId} marked as broken - Reason: {Reason}",
            request.PromiseId, request.Reason);

        return new MarkPromiseBrokenResponse(
            promiseToPay.Id,
            promiseToPay.Status,
            promiseToPay.BreachReason!);
    }
}
