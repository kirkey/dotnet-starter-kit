using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Search.v1;

public class SearchLoanCollateralsCommand : PaginationFilter, IRequest<PagedList<LoanCollateralResponse>>
{
    public Guid? LoanId { get; set; }
    public string? CollateralType { get; set; }
    public string? Status { get; set; }
}
