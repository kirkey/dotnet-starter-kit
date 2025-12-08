using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Search.v1;

public class SearchCollectionActionsCommand : PaginationFilter, IRequest<PagedList<CollectionActionSummaryResponse>>
{
    public DefaultIdType? CollectionCaseId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? PerformedById { get; set; }
    public string? ActionType { get; set; }
    public string? Outcome { get; set; }
    public DateTime? ActionDateFrom { get; set; }
    public DateTime? ActionDateTo { get; set; }
    public DateOnly? NextFollowUpDateFrom { get; set; }
    public DateOnly? NextFollowUpDateTo { get; set; }
}

public sealed record CollectionActionSummaryResponse(
    DefaultIdType Id,
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    string ActionType,
    DateTime ActionDateTime,
    DefaultIdType PerformedById,
    string? ContactMethod,
    string? PhoneNumberCalled,
    string? ContactPerson,
    string Outcome,
    decimal? PromisedAmount,
    DateOnly? PromisedDate,
    DateOnly? NextFollowUpDate,
    int? DurationMinutes,
    decimal? Latitude,
    decimal? Longitude
);
