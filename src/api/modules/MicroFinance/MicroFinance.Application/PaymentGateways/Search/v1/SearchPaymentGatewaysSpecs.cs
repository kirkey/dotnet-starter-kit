// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/PaymentGateways/Search/v1/SearchPaymentGatewaysSpecs.cs
using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Search.v1;

/// <summary>
/// Specification for searching payment gateways.
/// </summary>
public sealed class SearchPaymentGatewaysSpecs : Specification<PaymentGateway, PaymentGatewayResponse>
{
    public SearchPaymentGatewaysSpecs(SearchPaymentGatewaysCommand command)
    {
        Query.OrderBy(g => g.Name);

        if (!string.IsNullOrEmpty(command.Name))
        {
            Query.Where(g => g.Name.Contains(command.Name));
        }

        if (!string.IsNullOrEmpty(command.Provider))
        {
            Query.Where(g => g.Provider == command.Provider);
        }

        if (!string.IsNullOrEmpty(command.Status))
        {
            Query.Where(g => g.Status == command.Status);
        }

        if (command.SupportsMobileWallet.HasValue)
        {
            Query.Where(g => g.SupportsMobileWallet == command.SupportsMobileWallet.Value);
        }

        if (command.SupportsCardPayments.HasValue)
        {
            Query.Where(g => g.SupportsCardPayments == command.SupportsCardPayments.Value);
        }

        if (command.IsTestMode.HasValue)
        {
            Query.Where(g => g.IsTestMode == command.IsTestMode.Value);
        }

        Query.Skip(command.PageSize * (command.PageNumber - 1))
            .Take(command.PageSize);

        Query.Select(g => new PaymentGatewayResponse(
            g.Id,
            g.Name,
            g.Provider,
            g.Status,
            g.MerchantId,
            g.WebhookUrl,
            g.TransactionFeePercent,
            g.TransactionFeeFixed,
            g.MinTransactionAmount,
            g.MaxTransactionAmount,
            g.SupportsRefunds,
            g.SupportsRecurring,
            g.SupportsMobileWallet,
            g.SupportsCardPayments,
            g.SupportsBankTransfer,
            g.IsTestMode,
            g.TimeoutSeconds,
            g.RetryAttempts,
            g.LastSuccessfulConnection,
            g.CreatedOn));
    }
}

