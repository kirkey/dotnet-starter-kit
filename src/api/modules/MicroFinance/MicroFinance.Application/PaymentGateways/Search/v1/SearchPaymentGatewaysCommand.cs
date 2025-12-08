// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/PaymentGateways/Search/v1/SearchPaymentGatewaysCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Search.v1;

/// <summary>
/// Command for searching payment gateways with pagination and filters.
/// </summary>
public class SearchPaymentGatewaysCommand : PaginationFilter, IRequest<PagedList<PaymentGatewayResponse>>
{
    /// <summary>
    /// Filter by gateway name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filter by provider.
    /// </summary>
    public string? Provider { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by whether supports mobile wallet.
    /// </summary>
    public bool? SupportsMobileWallet { get; set; }

    /// <summary>
    /// Filter by whether supports card payments.
    /// </summary>
    public bool? SupportsCardPayments { get; set; }

    /// <summary>
    /// Filter by test mode.
    /// </summary>
    public bool? IsTestMode { get; set; }
}

