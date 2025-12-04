using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Search.v1;

public class SearchLoanProductsCommand : PaginationFilter, IRequest<PagedList<LoanProductResponse>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? InterestMethod { get; set; }
    public string? RepaymentFrequency { get; set; }
    public decimal? MinInterestRate { get; set; }
    public decimal? MaxInterestRate { get; set; }
    public bool? IsActive { get; set; }
}
