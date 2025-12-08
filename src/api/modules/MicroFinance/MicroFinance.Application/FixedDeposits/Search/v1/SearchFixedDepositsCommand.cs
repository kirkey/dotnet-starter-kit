using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Search.v1;

public class SearchFixedDepositsCommand : PaginationFilter, IRequest<PagedList<FixedDepositResponse>>
{
    public string? CertificateNumber { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public string? Status { get; set; }
    public DateOnly? DepositDateFrom { get; set; }
    public DateOnly? DepositDateTo { get; set; }
    public DateOnly? MaturityDateFrom { get; set; }
    public DateOnly? MaturityDateTo { get; set; }
    public decimal? MinPrincipal { get; set; }
    public decimal? MaxPrincipal { get; set; }
}
