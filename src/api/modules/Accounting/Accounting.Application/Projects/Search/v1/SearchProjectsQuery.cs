namespace Accounting.Application.Projects.Search.v1;

/// <summary>
/// Query to search projects with filtering, sorting, and pagination capabilities.
/// </summary>
/// <param name="SearchTerm">Optional search term to filter projects by name, description, or client</param>
/// <param name="Status">Optional status filter (Active, Completed, On Hold, Cancelled)</param>
/// <param name="Department">Optional department filter</param>
/// <param name="ProjectManager">Optional project manager filter</param>
/// <param name="StartDateFrom">Optional filter for projects starting from this date</param>
/// <param name="StartDateTo">Optional filter for projects starting before this date</param>
/// <param name="BudgetAmountFrom">Optional filter for minimum budget amount</param>
/// <param name="BudgetAmountTo">Optional filter for maximum budget amount</param>
/// <param name="PageNumber">Page number for pagination (default: 1)</param>
/// <param name="PageSize">Page size for pagination (default: 10)</param>
/// <param name="SortBy">Field to sort by (default: Name)</param>
/// <param name="SortDirection">Sort direction (Asc or Desc, default: Asc)</param>
public sealed record SearchProjectsQuery(
    string? SearchTerm = null,
    string? Status = null,
    string? Department = null,
    string? ProjectManager = null,
    DateTime? StartDateFrom = null,
    DateTime? StartDateTo = null,
    decimal? BudgetAmountFrom = null,
    decimal? BudgetAmountTo = null,
    int PageNumber = 1,
    int PageSize = 10,
    string SortBy = "Name",
    string SortDirection = "Asc") : IRequest<SearchProjectsResponse>;
