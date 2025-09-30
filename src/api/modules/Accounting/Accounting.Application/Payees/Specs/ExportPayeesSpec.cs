using Accounting.Application.Payees.Export.v1;

namespace Accounting.Application.Payees.Specs;

/// <summary>
/// Specification for exporting Payees with comprehensive filtering options.
/// Handles expense account, search terms, TIN presence, and payee status filtering.
/// </summary>
public sealed class ExportPayeesSpec : Specification<Payee>
{
    /// <summary>
    /// Initializes the specification with export query filters.
    /// </summary>
    /// <param name="query">Export query containing filter criteria</param>
    public ExportPayeesSpec(ExportPayeesQuery query)
    {
        // Filter by expense account code if specified
        if (!string.IsNullOrWhiteSpace(query.ExpenseAccountCode))
        {
            Query.Where(x => x.ExpenseAccountCode == query.ExpenseAccountCode);
        }

        // Filter by search term across multiple fields
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();
            Query.Where(x => x.PayeeCode.Contains(searchTerm) ||
                           x.Name.Contains(searchTerm) ||
                           (x.Tin != null && x.Tin.Contains(searchTerm)) ||
                           (x.Description != null && x.Description.Contains(searchTerm)));
        }

        // Filter by active status
        if (!query.IncludeInactive)
        {
            Query.Where(x => x.IsActive);
        }

        // Filter by TIN presence
        if (query.HasTin.HasValue)
        {
            if (query.HasTin.Value)
            {
                Query.Where(x => x.Tin != null && x.Tin != string.Empty);
            }
            else
            {
                Query.Where(x => x.Tin == null || x.Tin == string.Empty);
            }
        }

        // Filter by expense account mapping presence
        if (query.HasExpenseAccount.HasValue)
        {
            if (query.HasExpenseAccount.Value)
            {
                Query.Where(x => x.ExpenseAccountCode != null && x.ExpenseAccountCode != string.Empty);
            }
            else
            {
                Query.Where(x => x.ExpenseAccountCode == null || x.ExpenseAccountCode == string.Empty);
            }
        }

        // Order by payee code for consistent export structure
        Query.OrderBy(x => x.PayeeCode);
    }
}
