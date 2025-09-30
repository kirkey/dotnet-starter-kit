using Accounting.Application.ChartOfAccounts.Export.v1;

namespace Accounting.Application.ChartOfAccounts.Specs;

/// <summary>
/// Specification for exporting Chart of Accounts with comprehensive filtering options.
/// Handles account type, USOA category, search terms, and account hierarchy filtering.
/// </summary>
public sealed class ExportChartOfAccountsSpec : Specification<ChartOfAccount>
{
    /// <summary>
    /// Initializes the specification with export query filters.
    /// </summary>
    /// <param name="query">Export query containing filter criteria</param>
    public ExportChartOfAccountsSpec(ExportChartOfAccountsQuery query)
    {
        // Filter by account type if specified
        if (!string.IsNullOrWhiteSpace(query.AccountType))
        {
            Query.Where(x => x.AccountType == query.AccountType);
        }

        // Filter by USOA category if specified
        if (!string.IsNullOrWhiteSpace(query.UsoaCategory))
        {
            Query.Where(x => x.UsoaCategory == query.UsoaCategory);
        }

        // Filter by search term across multiple fields
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();
            Query.Where(x => x.AccountCode.Contains(searchTerm) ||
                           x.AccountName.Contains(searchTerm) ||
                           (x.Description != null && x.Description.Contains(searchTerm)));
        }

        // Filter by active status
        if (!query.IncludeInactive)
        {
            Query.Where(x => x.IsActive);
        }

        // Filter by control account status
        if (query.OnlyControlAccounts)
        {
            Query.Where(x => x.IsControlAccount);
        }
        else if (query.OnlyDetailAccounts)
        {
            Query.Where(x => !x.IsControlAccount);
        }

        // Filter by parent account if specified
        if (query.ParentAccountId.HasValue)
        {
            Query.Where(x => x.ParentAccountId == query.ParentAccountId);
        }

        // Order by account code for consistent export structure
        Query.OrderBy(x => x.AccountCode);
    }
}
