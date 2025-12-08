using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Search.v1;

public class SearchCollateralInsurancesCommand : PaginationFilter, IRequest<PagedList<CollateralInsuranceSummaryResponse>>
{
    public DefaultIdType? CollateralId { get; set; }
    public string? PolicyNumber { get; set; }
    public string? InsurerName { get; set; }
    public string? InsuranceType { get; set; }
    public string? Status { get; set; }
    public DateOnly? EffectiveDateFrom { get; set; }
    public DateOnly? EffectiveDateTo { get; set; }
    public DateOnly? ExpiryDateFrom { get; set; }
    public DateOnly? ExpiryDateTo { get; set; }
}

public sealed record CollateralInsuranceSummaryResponse(
    DefaultIdType Id,
    DefaultIdType CollateralId,
    string PolicyNumber,
    string InsurerName,
    string InsuranceType,
    string Status,
    decimal CoverageAmount,
    decimal PremiumAmount,
    DateOnly EffectiveDate,
    DateOnly ExpiryDate,
    bool IsMfiAsBeneficiary);
