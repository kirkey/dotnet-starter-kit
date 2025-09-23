using Accounting.Application.Projects.Responses;

namespace Accounting.Application.Projects.Search.v1;

public class SearchProjectsCommand : PaginationFilter, IRequest<PagedList<ProjectResponse>>
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? Department { get; set; }
    public string? ProjectManager { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public decimal? BudgetAmountFrom { get; set; }
    public decimal? BudgetAmountTo { get; set; }
}

