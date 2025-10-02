using Accounting.Application.TaxCodes.Responses;

namespace Accounting.Application.TaxCodes.Search.v1;

public class SearchTaxCodesCommand : PaginationFilter, IRequest<PagedList<TaxCodeResponse>>
{
    public string? Code { get; set; }
    public string? TaxType { get; set; }
    public string? Jurisdiction { get; set; }
    public bool? IsActive { get; set; }
}
