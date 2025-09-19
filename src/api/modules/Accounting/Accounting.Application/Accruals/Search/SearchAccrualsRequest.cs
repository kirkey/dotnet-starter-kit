using Accounting.Application.Accruals.Dtos;

namespace Accounting.Application.Accruals.Search;

public class SearchAccrualsQuery : PaginationFilter, IRequest<PagedList<AccrualDto>>
{
    public string? ReferenceNumber { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
