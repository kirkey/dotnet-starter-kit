using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Reverse.v1;

/// <summary>
/// Handler for reversing a fee payment.
/// </summary>
public sealed class ReverseFeePaymentHandler(
    [FromKeyedServices("microfinance:feepayments")] IRepository<FeePayment> repository,
    ILogger<ReverseFeePaymentHandler> logger)
    : IRequestHandler<ReverseFeePaymentCommand, ReverseFeePaymentResponse>
{
    public async Task<ReverseFeePaymentResponse> Handle(ReverseFeePaymentCommand request, CancellationToken cancellationToken)
    {
        var feePayment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (feePayment is null)
        {
            throw new NotFoundException($"Fee payment with ID {request.Id} not found.");
        }

        feePayment.Reverse(request.Reason);

        await repository.UpdateAsync(feePayment, cancellationToken);

        logger.LogInformation("Fee payment {FeePaymentId} reversed. Reason: {Reason}", request.Id, request.Reason);

        return new ReverseFeePaymentResponse(feePayment.Id, feePayment.Status);
    }
}
