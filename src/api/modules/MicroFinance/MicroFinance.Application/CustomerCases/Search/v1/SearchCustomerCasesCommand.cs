using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Search.v1;

public class SearchCustomerCasesCommand : PaginationFilter, IRequest<PagedList<CustomerCaseSummaryResponse>>
{
    public string? CaseNumber { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public string? Category { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public string? Channel { get; set; }
    public DefaultIdType? AssignedToId { get; set; }
    public DateOnly? OpenedFrom { get; set; }
    public DateOnly? OpenedTo { get; set; }
    public bool? SlaBreached { get; set; }
}

public sealed record CustomerCaseSummaryResponse(
    DefaultIdType Id,
    string CaseNumber,
    DefaultIdType MemberId,
    string Subject,
    string Category,
    string Priority,
    string Status,
    string Channel,
    DefaultIdType? AssignedToId,
    DateTimeOffset OpenedAt,
    DateTimeOffset? ResolvedAt,
    bool SlaBreached,
    int? CustomerSatisfactionScore
);
