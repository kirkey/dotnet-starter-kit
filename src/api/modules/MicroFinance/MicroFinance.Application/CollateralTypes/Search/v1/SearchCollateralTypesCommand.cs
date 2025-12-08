using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Search.v1;

public class SearchCollateralTypesCommand : PaginationFilter, IRequest<PagedList<CollateralTypeSummaryResponse>>
{
    public string? Code { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public bool? RequiresInsurance { get; set; }
    public bool? RequiresAppraisal { get; set; }
    public bool? RequiresRegistration { get; set; }
}

public sealed record CollateralTypeSummaryResponse(
    DefaultIdType Id,
    string Name,
    string Code,
    string Category,
    string Status,
    decimal DefaultLtvPercent,
    decimal MaxLtvPercent,
    int DefaultUsefulLifeYears,
    decimal AnnualDepreciationRate,
    bool RequiresInsurance,
    bool RequiresAppraisal,
    int RevaluationFrequencyMonths,
    bool RequiresRegistration,
    int DisplayOrder
);
