using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;

public sealed class GetMobileTransactionHandler(
    [FromKeyedServices("microfinance:mobiletransactions")] IReadRepository<MobileTransaction> repository)
    : IRequestHandler<GetMobileTransactionRequest, MobileTransactionResponse>
{
    public async Task<MobileTransactionResponse> Handle(
        GetMobileTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await repository.FirstOrDefaultAsync(
            new MobileTransactionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Mobile transaction {request.Id} not found");

        return new MobileTransactionResponse(
            transaction.Id,
            transaction.WalletId,
            transaction.TransactionReference,
            transaction.TransactionType,
            transaction.Status,
            transaction.Amount,
            transaction.Fee,
            transaction.NetAmount,
            transaction.SourcePhone,
            transaction.DestinationPhone,
            transaction.RecipientWalletId,
            transaction.LinkedLoanId,
            transaction.LinkedSavingsAccountId,
            transaction.ProviderReference,
            transaction.InitiatedAt,
            transaction.CompletedAt,
            transaction.FailureReason);
    }
}
