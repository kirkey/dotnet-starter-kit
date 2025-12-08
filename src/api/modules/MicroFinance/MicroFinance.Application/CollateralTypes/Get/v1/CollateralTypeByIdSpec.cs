using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Get.v1;

/// <summary>
/// Specification for getting a collateral type by ID.
/// </summary>
public sealed class CollateralTypeByIdSpec : Specification<CollateralType, CollateralTypeResponse>
{
    public CollateralTypeByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);

        Query.Select(c => new CollateralTypeResponse(
            c.Id,
            c.Name,
            c.Code,
            c.Category,
            c.Description,
            c.Status,
            c.DefaultLtvPercent,
            c.MaxLtvPercent,
            c.DefaultUsefulLifeYears,
            c.AnnualDepreciationRate,
            c.RequiresInsurance,
            c.RequiresAppraisal));
    }
}
