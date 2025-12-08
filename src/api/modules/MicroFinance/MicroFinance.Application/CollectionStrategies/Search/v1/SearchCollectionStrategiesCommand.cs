using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Search.v1;

public class SearchCollectionStrategiesCommand : PaginationFilter, IRequest<PagedList<CollectionStrategySummaryResponse>>
{
    public string? Code { get; set; }
    public DefaultIdType? LoanProductId { get; set; }
    public string? ActionType { get; set; }
    public int? MinDaysPastDue { get; set; }
    public int? MaxDaysPastDue { get; set; }
    public bool? IsActive { get; set; }
    public bool? RequiresApproval { get; set; }
}

public sealed record CollectionStrategySummaryResponse(
    DefaultIdType Id,
    string Name,
    string Code,
    DefaultIdType? LoanProductId,
    int TriggerDaysPastDue,
    int? MaxDaysPastDue,
    decimal? MinOutstandingAmount,
    decimal? MaxOutstandingAmount,
    string ActionType,
    int Priority,
    int? RepeatIntervalDays,
    int? MaxRepetitions,
    bool EscalateOnFailure,
    bool RequiresApproval,
    bool IsActive
);
