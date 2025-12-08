// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/MobileTransactions/Search/v1/SearchMobileTransactionsSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Search.v1;

/// <summary>
/// Specification for searching mobile transactions.
/// </summary>
public sealed class SearchMobileTransactionsSpecs : Specification<MobileTransaction, MobileTransactionResponse>
{
    public SearchMobileTransactionsSpecs(SearchMobileTransactionsCommand command)
    {
        Query.OrderByDescending(t => t.InitiatedAt);

        if (command.WalletId.HasValue)
        {
            Query.Where(t => t.WalletId == command.WalletId.Value);
        }

        if (!string.IsNullOrEmpty(command.TransactionType))
        {
            Query.Where(t => t.TransactionType == command.TransactionType);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(t => t.Status == command.Status);
        }

        if (!string.IsNullOrEmpty(command.ReferenceNumber))
        {
            Query.Where(t => t.TransactionReference.Contains(command.ReferenceNumber));
        }

        if (command.DateFrom.HasValue)
        {
            var fromDate = command.DateFrom.Value.ToDateTime(TimeOnly.MinValue);
            Query.Where(t => t.InitiatedAt >= fromDate);
        }

        if (command.DateTo.HasValue)
        {
            var toDate = command.DateTo.Value.ToDateTime(TimeOnly.MaxValue);
            Query.Where(t => t.InitiatedAt <= toDate);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(t => new MobileTransactionResponse(
            t.Id,
            t.WalletId,
            t.TransactionReference,
            t.TransactionType,
            t.Status,
            t.Amount,
            t.Fee,
            t.NetAmount,
            t.SourcePhone,
            t.DestinationPhone,
            t.RecipientWalletId,
            t.LinkedLoanId,
            t.LinkedSavingsAccountId,
            t.ProviderReference,
            t.InitiatedAt,
            t.CompletedAt,
            t.FailureReason));
    }
}

