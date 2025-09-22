#if false
using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Queries;

public class SearchAccrualsQuery : IRequest<List<AccrualResponse>>
{
    public string? AccrualNumber { get; set; }
    public string? Description { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
#endif
