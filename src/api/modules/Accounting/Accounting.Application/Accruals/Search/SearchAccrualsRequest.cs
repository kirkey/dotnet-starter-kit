using Accounting.Application.Accruals.Dtos;

namespace Accounting.Application.Accruals.Search;

public class SearchAccrualsRequest : IRequest<List<AccrualDto>>
{
    public string? AccrualNumber { get; set; }
    public string? Description { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

