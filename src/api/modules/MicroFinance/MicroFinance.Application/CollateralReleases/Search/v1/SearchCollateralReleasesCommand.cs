using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Search.v1;

public class SearchCollateralReleasesCommand : PaginationFilter, IRequest<PagedList<CollateralReleaseSummaryResponse>>
{
    public DefaultIdType? CollateralId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public string? ReleaseReference { get; set; }
    public string? Status { get; set; }
    public string? ReleaseMethod { get; set; }
    public DateOnly? RequestDateFrom { get; set; }
    public DateOnly? RequestDateTo { get; set; }
    public DateOnly? ReleasedDateFrom { get; set; }
    public DateOnly? ReleasedDateTo { get; set; }
    public DefaultIdType? RequestedById { get; set; }
}

public sealed record CollateralReleaseSummaryResponse(
    DefaultIdType Id,
    DefaultIdType CollateralId,
    DefaultIdType LoanId,
    string ReleaseReference,
    string Status,
    DateOnly RequestDate,
    string? ReleaseMethod,
    string? RecipientName,
    DateOnly? ReleasedDate,
    bool DocumentsReturned);
