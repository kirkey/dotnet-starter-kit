using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.RecordPayment.v1;

/// <summary>
/// Handler for recording fee payment.
/// </summary>
public sealed class RecordFeePaymentHandler(
    [FromKeyedServices("microfinance:feecharges")] IRepository<FeeCharge> repository,
    ILogger<RecordFeePaymentHandler> logger)
    : IRequestHandler<RecordFeePaymentCommand, RecordFeePaymentResponse>
{
    public async Task<RecordFeePaymentResponse> Handle(RecordFeePaymentCommand request, CancellationToken cancellationToken)
    {
        var feeCharge = await repository.GetByIdAsync(request.FeeChargeId, cancellationToken)
            ?? throw new NotFoundException($"Fee charge with ID {request.FeeChargeId} not found.");

        feeCharge.RecordPayment(request.Amount);

        await repository.UpdateAsync(feeCharge, cancellationToken);
        logger.LogInformation("Recorded payment {Amount} for fee charge {FeeChargeId}", request.Amount, request.FeeChargeId);

        return new RecordFeePaymentResponse(
            feeCharge.Id,
            feeCharge.AmountPaid,
            feeCharge.Outstanding,
            feeCharge.Status,
            "Payment recorded successfully.");
    }
}
