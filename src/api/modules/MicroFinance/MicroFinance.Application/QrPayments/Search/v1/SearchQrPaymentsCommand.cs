// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/QrPayments/Search/v1/SearchQrPaymentsCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Search.v1;

/// <summary>
/// Command for searching QR payments with pagination and filters.
/// </summary>
public class SearchQrPaymentsCommand : PaginationFilter, IRequest<PagedList<QrPaymentResponse>>
{
    /// <summary>
    /// Filter by wallet ID.
    /// </summary>
    public DefaultIdType? WalletId { get; set; }

    /// <summary>
    /// Filter by member ID.
    /// </summary>
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Filter by agent ID.
    /// </summary>
    public DefaultIdType? AgentId { get; set; }

    /// <summary>
    /// Filter by QR type.
    /// </summary>
    public string? QrType { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by generated date from.
    /// </summary>
    public DateOnly? GeneratedFrom { get; set; }

    /// <summary>
    /// Filter by generated date to.
    /// </summary>
    public DateOnly? GeneratedTo { get; set; }
}

