namespace Accounting.Application.Projects.Search.v1;

/// <summary>
/// Response for the search projects query with paginated results and summary information.
/// </summary>
/// <param name="Projects">List of projects matching the search criteria</param>
/// <param name="TotalCount">Total number of projects matching the criteria</param>
/// <param name="PageNumber">Current page number</param>
/// <param name="PageSize">Number of items per page</param>
/// <param name="TotalPages">Total number of pages</param>
/// <param name="HasPreviousPage">Indicates if there's a previous page</param>
/// <param name="HasNextPage">Indicates if there's a next page</param>
public sealed record SearchProjectsResponse(
    IEnumerable<ProjectSearchResult> Projects,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);

/// <summary>
/// Project search result with essential information for list display.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project</param>
/// <param name="Name">The name of the project</param>
/// <param name="Status">The current project status</param>
/// <param name="StartDate">The project start date</param>
/// <param name="EndDate">The project end date (if completed/cancelled)</param>
/// <param name="BudgetedAmount">The approved budget amount</param>
/// <param name="ActualCost">The current actual cost</param>
/// <param name="ActualRevenue">The current actual revenue</param>
/// <param name="BudgetVariance">The current budget variance</param>
/// <param name="BudgetUtilizationPercentage">The percentage of budget utilized</param>
/// <param name="ClientName">The client or customer name</param>
/// <param name="ProjectManager">The assigned project manager</param>
/// <param name="Department">The owning department</param>
/// <param name="CostEntriesCount">Number of associated cost entries</param>
public sealed record ProjectSearchResult(
    DefaultIdType ProjectId,
    string Name,
    string Status,
    DateTime StartDate,
    DateTime? EndDate,
    decimal BudgetedAmount,
    decimal ActualCost,
    decimal ActualRevenue,
    decimal BudgetVariance,
    decimal BudgetUtilizationPercentage,
    string? ClientName,
    string? ProjectManager,
    string? Department,
    int CostEntriesCount);
