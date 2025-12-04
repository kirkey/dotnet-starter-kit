using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Search.v1;

public class SearchLoanGuarantorsCommand : PaginationFilter, IRequest<PagedList<LoanGuarantorResponse>>
{
    public Guid? LoanId { get; set; }
    public Guid? GuarantorMemberId { get; set; }
    public string? Status { get; set; }
}
