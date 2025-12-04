using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.RecordPayment.v1;

public sealed class RecordSettlementPaymentHandler(
    [FromKeyedServices("microfinance:debtsettlements")] IRepository<DebtSettlement> repository,
    ILogger<RecordSettlementPaymentHandler> logger)
    : IRequestHandler<RecordSettlementPaymentCommand, RecordSettlementPaymentResponse>
{
    public async Task<RecordSettlementPaymentResponse> Handle(
        RecordSettlementPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var settlement = await repository.FirstOrDefaultAsync(
            new DebtSettlementByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Debt settlement {request.Id} not found");

        settlement.RecordPayment(request.Amount);
        await repository.UpdateAsync(settlement, cancellationToken);

        logger.LogInformation("Payment recorded for settlement: {SettlementId}, Amount: {Amount}",
            settlement.Id, request.Amount);

        return new RecordSettlementPaymentResponse(
            settlement.Id,
            settlement.Status,
            settlement.AmountPaid,
            settlement.RemainingBalance);
    }
}
