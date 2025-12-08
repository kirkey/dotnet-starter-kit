using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Search.v1;

public class SearchFeeChargesCommand : PaginationFilter, IRequest<PagedList<FeeChargeResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? SavingsAccountId { get; set; }
    public DefaultIdType? FeeDefinitionId { get; set; }
    public string? Status { get; set; }
    public DateOnly? ChargeDateFrom { get; set; }
    public DateOnly? ChargeDateTo { get; set; }
}
