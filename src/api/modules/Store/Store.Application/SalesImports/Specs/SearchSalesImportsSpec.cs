using FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

/// <summary>
/// Specification for searching sales imports with filters.
/// </summary>
public class SearchSalesImportsSpec : Specification<SalesImport>
{
    public SearchSalesImportsSpec(SearchSalesImportsRequest request)
    {
        Query
            .Include(x => x.Warehouse)
            .OrderByDescending(x => x.ImportDate);

        if (!string.IsNullOrWhiteSpace(request.ImportNumber))
        {
            Query.Where(x => x.ImportNumber.Contains(request.ImportNumber));
        }

        if (request.WarehouseId.HasValue && request.WarehouseId != default)
        {
            Query.Where(x => x.WarehouseId == request.WarehouseId);
        }

        if (request.SalesPeriodFrom.HasValue)
        {
            Query.Where(x => x.SalesPeriodFrom >= request.SalesPeriodFrom);
        }

        if (request.SalesPeriodTo.HasValue)
        {
            Query.Where(x => x.SalesPeriodTo <= request.SalesPeriodTo);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(x => x.Status == request.Status.ToUpper());
        }

        if (request.IsReversed.HasValue)
        {
            Query.Where(x => x.IsReversed == request.IsReversed);
        }

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            Query.Where(x => 
                x.ImportNumber.Contains(request.Keyword) ||
                x.FileName.Contains(request.Keyword) ||
                (x.Notes != null && x.Notes.Contains(request.Keyword)));
        }
    }
}

