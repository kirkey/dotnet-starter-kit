using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Update.v1;

/// <summary>
/// Handler for updating a fee payment.
/// </summary>
public sealed class UpdateFeePaymentHandler(
    [FromKeyedServices("microfinance:feepayments")] IRepository<FeePayment> repository,
    ILogger<UpdateFeePaymentHandler> logger)
    : IRequestHandler<UpdateFeePaymentCommand, UpdateFeePaymentResponse>
{
    public async Task<UpdateFeePaymentResponse> Handle(UpdateFeePaymentCommand request, CancellationToken cancellationToken)
    {
        var feePayment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (feePayment is null)
        {
            throw new NotFoundException($"Fee payment with ID {request.Id} not found.");
        }

        feePayment.Update(
            request.Reference,
            request.PaymentDate,
            request.Amount,
            request.PaymentMethod,
            request.PaymentSource,
            request.Notes);

        await repository.UpdateAsync(feePayment, cancellationToken);

        logger.LogInformation("Fee payment {FeePaymentId} updated", request.Id);

        return new UpdateFeePaymentResponse(feePayment.Id);
    }
}
