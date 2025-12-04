using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Create.v1;

/// <summary>
/// Handler for creating a new promise to pay.
/// </summary>
public sealed class CreatePromiseToPayHandler(
    [FromKeyedServices("microfinance:promisetopays")] IRepository<PromiseToPay> repository,
    ILogger<CreatePromiseToPayHandler> logger)
    : IRequestHandler<CreatePromiseToPayCommand, CreatePromiseToPayResponse>
{
    public async Task<CreatePromiseToPayResponse> Handle(CreatePromiseToPayCommand request, CancellationToken cancellationToken)
    {
        var promiseToPay = PromiseToPay.Create(
            request.CollectionCaseId,
            request.LoanId,
            request.MemberId,
            request.PromisedPaymentDate,
            request.PromisedAmount,
            request.RecordedById,
            request.CollectionActionId);

        await repository.AddAsync(promiseToPay, cancellationToken);

        logger.LogInformation("Promise to pay created for member {MemberId} - Amount: {Amount}, Date: {Date}",
            request.MemberId, request.PromisedAmount, request.PromisedPaymentDate);

        return new CreatePromiseToPayResponse(
            promiseToPay.Id,
            promiseToPay.PromiseDate,
            promiseToPay.PromisedPaymentDate,
            promiseToPay.PromisedAmount,
            promiseToPay.Status);
    }
}
