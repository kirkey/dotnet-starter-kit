using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Get.v1;

public sealed class GetSavingsTransactionHandler(
    [FromKeyedServices("microfinance:savingstransactions")] IRepository<SavingsTransaction> repository)
    : IRequestHandler<GetSavingsTransactionRequest, SavingsTransactionResponse>
{
    public async Task<SavingsTransactionResponse> Handle(GetSavingsTransactionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transaction = await repository.FirstOrDefaultAsync(
            new SavingsTransactionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (transaction is null)
        {
            throw new NotFoundException($"Savings transaction with ID {request.Id} not found.");
        }

        return new SavingsTransactionResponse(
            transaction.Id,
            transaction.SavingsAccountId,
            transaction.Reference,
            transaction.TransactionType,
            transaction.Amount,
            transaction.BalanceAfter,
            transaction.TransactionDate,
            transaction.Description,
            transaction.PaymentMethod
        );
    }
}
