using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Get.v1;

public sealed class GetInvestmentTransactionHandler(
    [FromKeyedServices("microfinance:investmenttransactions")] IReadRepository<InvestmentTransaction> repository,
    ILogger<GetInvestmentTransactionHandler> logger)
    : IRequestHandler<GetInvestmentTransactionRequest, InvestmentTransactionResponse>
{
    public async Task<InvestmentTransactionResponse> Handle(GetInvestmentTransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = await repository.FirstOrDefaultAsync(new InvestmentTransactionByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Investment transaction {request.Id} not found");

        logger.LogInformation("Retrieved investment transaction {Id}", transaction.Id);

        return new InvestmentTransactionResponse(
            transaction.Id,
            transaction.InvestmentAccountId,
            transaction.ProductId,
            transaction.TransactionReference,
            transaction.TransactionType,
            transaction.Status,
            transaction.Amount,
            transaction.Units,
            transaction.NavAtTransaction,
            transaction.EntryLoadAmount,
            transaction.ExitLoadAmount,
            transaction.NetAmount,
            transaction.GainLoss,
            transaction.RequestedAt,
            transaction.ProcessedAt,
            transaction.AllotmentDate,
            transaction.PaymentMode,
            transaction.PaymentReference,
            transaction.Notes,
            transaction.FailureReason);
    }
}
