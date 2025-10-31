using Accounting.Domain.Entities;

namespace Accounting.Application.CostCenters.Queries;

/// <summary>
/// Specification to find cost center by code.
/// </summary>
public class CostCenterByCodeSpec : Specification<CostCenter>
{
    public CostCenterByCodeSpec(string code)
    {
        Query.Where(c => c.Code == code);
    }
}

/// <summary>
/// Specification to find cost center by ID.
/// </summary>
public class CostCenterByIdSpec : Specification<CostCenter>
{
    public CostCenterByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}

/// <summary>
/// Specification for searching cost centers with filters.
/// </summary>
public class CostCenterSearchSpec : Specification<CostCenter>
{
    public CostCenterSearchSpec(
        string? code = null,
        string? name = null,
        string? costCenterType = null,
        bool? isActive = null,
        DefaultIdType? parentCostCenterId = null)
    {
        if (!string.IsNullOrWhiteSpace(code))
        {
            Query.Where(c => c.Code.Contains(code));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            Query.Where(c => c.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(costCenterType))
        {
            Query.Where(c => c.CostCenterType.ToString() == costCenterType);
        }

        if (isActive.HasValue)
        {
            Query.Where(c => c.IsActive == isActive.Value);
        }

        if (parentCostCenterId.HasValue)
        {
            Query.Where(c => c.ParentCostCenterId == parentCostCenterId.Value);
        }

        Query.OrderBy(c => c.Code);
    }
}

