using Accounting.Application.Projects.Specifications;

namespace Accounting.Application.Projects.Search.v1;

/// <summary>
/// Handler for searching projects with advanced filtering, sorting, and pagination.
/// </summary>
public sealed class SearchProjectsHandler(
    ILogger<SearchProjectsHandler> logger,
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository)
    : IRequestHandler<SearchProjectsQuery, SearchProjectsResponse>
{
    public async Task<SearchProjectsResponse> Handle(SearchProjectsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new ProjectSearchSpec(request);
        var projects = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("searched projects with {TotalCount} results", totalCount);

        var projectResults = projects.Select(p => new ProjectSearchResult(
            ProjectId: p.Id,
            Name: p.Name,
            Status: p.Status,
            StartDate: p.StartDate,
            EndDate: p.EndDate,
            BudgetedAmount: p.BudgetedAmount,
            ActualCost: p.ActualCost,
            ActualRevenue: p.ActualRevenue,
            BudgetVariance: p.GetBudgetVariance(),
            BudgetUtilizationPercentage: p.GetBudgetUtilizationPercentage(),
            ClientName: p.ClientName,
            ProjectManager: p.ProjectManager,
            Department: p.Department,
            CostEntriesCount: p.CostingEntries.Count));

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new SearchProjectsResponse(
            Projects: projectResults,
            TotalCount: totalCount,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            TotalPages: totalPages,
            HasPreviousPage: request.PageNumber > 1,
            HasNextPage: request.PageNumber < totalPages);
    }
}
