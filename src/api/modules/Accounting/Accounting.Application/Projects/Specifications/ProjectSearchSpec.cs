using Accounting.Application.Projects.Search.v1;

namespace Accounting.Application.Projects.Specifications;

/// <summary>
/// Specification for searching projects with comprehensive filtering, sorting, and pagination.
/// </summary>
public class ProjectSearchSpec : Specification<Project>
{
    public ProjectSearchSpec(SearchProjectsQuery query)
    {
        // Include cost entries for count
        Query.Include(p => p.CostingEntries);

        // Apply search term filter
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            Query.Where(p => p.Name.Contains(query.SearchTerm) ||
                           (p.Description != null && p.Description.Contains(query.SearchTerm)) ||
                           (p.ClientName != null && p.ClientName.Contains(query.SearchTerm)));
        }

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            Query.Where(p => p.Status == query.Status);
        }

        // Apply department filter
        if (!string.IsNullOrWhiteSpace(query.Department))
        {
            Query.Where(p => p.Department == query.Department);
        }

        // Apply project manager filter
        if (!string.IsNullOrWhiteSpace(query.ProjectManager))
        {
            Query.Where(p => p.ProjectManager == query.ProjectManager);
        }

        // Apply start date range filters
        if (query.StartDateFrom.HasValue)
        {
            Query.Where(p => p.StartDate >= query.StartDateFrom.Value);
        }

        if (query.StartDateTo.HasValue)
        {
            Query.Where(p => p.StartDate <= query.StartDateTo.Value);
        }

        // Apply budget amount range filters
        if (query.BudgetAmountFrom.HasValue)
        {
            Query.Where(p => p.BudgetedAmount >= query.BudgetAmountFrom.Value);
        }

        if (query.BudgetAmountTo.HasValue)
        {
            Query.Where(p => p.BudgetedAmount <= query.BudgetAmountTo.Value);
        }

        // Apply sorting
        switch (query.SortBy.ToLower())
        {
            case "name":
                Query.OrderBy(p => p.Name, query.SortDirection.ToLower() == "desc");
                break;
            case "startdate":
                Query.OrderBy(p => p.StartDate, query.SortDirection.ToLower() == "desc");
                break;
            case "status":
                Query.OrderBy(p => p.Status, query.SortDirection.ToLower() == "desc");
                break;
            case "budgetedamount":
                Query.OrderBy(p => p.BudgetedAmount, query.SortDirection.ToLower() == "desc");
                break;
            case "actualcost":
                Query.OrderBy(p => p.ActualCost, query.SortDirection.ToLower() == "desc");
                break;
            default:
                Query.OrderBy(p => p.Name, false); // Default ascending by name
                break;
        }

        // Apply pagination
        Query.Skip((query.PageNumber - 1) * query.PageSize)
             .Take(query.PageSize);
    }
}
