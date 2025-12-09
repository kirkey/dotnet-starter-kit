using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Create.v1;

/// <summary>
/// Handler for creating a new fee payment.
/// </summary>
public sealed class CreateFeePaymentHandler(
    [FromKeyedServices("microfinance:feepayments")] IRepository<FeePayment> repository,
    ILogger<CreateFeePaymentHandler> logger)
    : IRequestHandler<CreateFeePaymentCommand, CreateFeePaymentResponse>
{
    public async Task<CreateFeePaymentResponse> Handle(CreateFeePaymentCommand request, CancellationToken cancellationToken)
    {
        var feePayment = FeePayment.Create(
            request.FeeChargeId,
            request.Reference,
            request.Amount,
            request.PaymentMethod,
            request.PaymentSource,
            request.PaymentDate,
            request.LoanRepaymentId,
            request.SavingsTransactionId,
            request.Notes);

        await repository.AddAsync(feePayment, cancellationToken);

        logger.LogInformation("Fee payment {Reference} created for fee charge {FeeChargeId}, amount: {Amount}",
            request.Reference, request.FeeChargeId, request.Amount);

        return new CreateFeePaymentResponse(
            feePayment.Id,
            feePayment.Reference,
            feePayment.Amount,
            feePayment.Status);
    }
}
