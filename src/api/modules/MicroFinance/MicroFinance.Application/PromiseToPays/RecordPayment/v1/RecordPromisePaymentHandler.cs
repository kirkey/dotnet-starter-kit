using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.RecordPayment.v1;

/// <summary>
/// Handler for recording payment against a promise to pay.
/// </summary>
public sealed class RecordPromisePaymentHandler(
    [FromKeyedServices("microfinance:promisetopays")] IRepository<PromiseToPay> repository,
    ILogger<RecordPromisePaymentHandler> logger)
    : IRequestHandler<RecordPromisePaymentCommand, RecordPromisePaymentResponse>
{
    public async Task<RecordPromisePaymentResponse> Handle(RecordPromisePaymentCommand request, CancellationToken cancellationToken)
    {
        var promiseToPay = await repository.GetByIdAsync(request.PromiseId, cancellationToken);

        if (promiseToPay is null)
        {
            throw new NotFoundException($"Promise to pay with ID {request.PromiseId} not found.");
        }

        promiseToPay.RecordPayment(request.Amount, request.PaymentDate);
        await repository.UpdateAsync(promiseToPay, cancellationToken);

        logger.LogInformation("Payment {Amount} recorded for promise {PromiseId} - Status: {Status}",
            request.Amount, request.PromiseId, promiseToPay.Status);

        return new RecordPromisePaymentResponse(
            promiseToPay.Id,
            promiseToPay.ActualAmountPaid,
            promiseToPay.Status);
    }
}
