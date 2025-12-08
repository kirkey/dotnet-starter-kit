// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/QrPayments/Search/v1/SearchQrPaymentsSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Search.v1;

/// <summary>
/// Specification for searching QR payments.
/// </summary>
public sealed class SearchQrPaymentsSpecs : Specification<QrPayment, QrPaymentResponse>
{
    public SearchQrPaymentsSpecs(SearchQrPaymentsCommand command)
    {
        Query.OrderByDescending(q => q.GeneratedAt);

        if (command.WalletId.HasValue)
        {
            Query.Where(q => q.WalletId == command.WalletId.Value);
        }

        if (command.MemberId.HasValue)
        {
            Query.Where(q => q.MemberId == command.MemberId.Value);
        }

        if (command.AgentId.HasValue)
        {
            Query.Where(q => q.AgentId == command.AgentId.Value);
        }

        if (!string.IsNullOrEmpty(command.QrType))
        {
            Query.Where(q => q.QrType == command.QrType);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(q => q.Status == command.Status);
        }

        if (command.GeneratedFrom.HasValue)
        {
            var fromDate = command.GeneratedFrom.Value.ToDateTime(TimeOnly.MinValue);
            Query.Where(q => q.GeneratedAt >= fromDate);
        }

        if (command.GeneratedTo.HasValue)
        {
            var toDate = command.GeneratedTo.Value.ToDateTime(TimeOnly.MaxValue);
            Query.Where(q => q.GeneratedAt <= toDate);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(q => new QrPaymentResponse(
            q.Id,
            q.WalletId,
            q.MemberId,
            q.AgentId,
            q.QrCode,
            q.QrType,
            q.Status,
            q.Amount,
            q.Reference,
            q.Notes,
            q.MaxUses,
            q.CurrentUses,
            q.GeneratedAt,
            q.ExpiresAt,
            q.LastUsedAt,
            q.LastTransactionId));
    }
}

