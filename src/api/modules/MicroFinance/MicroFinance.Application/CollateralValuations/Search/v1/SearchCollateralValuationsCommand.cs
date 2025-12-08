using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Search.v1;

public class SearchCollateralValuationsCommand : PaginationFilter, IRequest<PagedList<CollateralValuationSummaryResponse>>
{
    public DefaultIdType? CollateralId { get; set; }
    public string? ValuationReference { get; set; }
    public string? Status { get; set; }
    public string? ValuationMethod { get; set; }
    public string? AppraiserName { get; set; }
    public DateOnly? ValuationDateFrom { get; set; }
    public DateOnly? ValuationDateTo { get; set; }
    public decimal? MinMarketValue { get; set; }
    public decimal? MaxMarketValue { get; set; }
}

public sealed record CollateralValuationSummaryResponse(
    DefaultIdType Id,
    DefaultIdType CollateralId,
    string ValuationReference,
    string Status,
    DateOnly ValuationDate,
    DateOnly? ExpiryDate,
    string ValuationMethod,
    string? AppraiserName,
    string? AppraiserCompany,
    decimal MarketValue,
    decimal ForcedSaleValue,
    decimal InsurableValue,
    decimal? PreviousValue,
    decimal? ValueChange,
    decimal? ValueChangePercent,
    string? Condition,
    DefaultIdType? ApprovedById,
    DateOnly? ApprovedDate
);
