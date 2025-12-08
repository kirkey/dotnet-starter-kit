using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Search.v1;

public class SearchApprovalRequestsCommand : PaginationFilter, IRequest<PagedList<ApprovalRequestSummaryResponse>>
{
    public string? RequestNumber { get; set; }
    public DefaultIdType? WorkflowId { get; set; }
    public string? EntityType { get; set; }
    public DefaultIdType? EntityId { get; set; }
    public string? Status { get; set; }
    public int? CurrentLevel { get; set; }
    public DefaultIdType? SubmittedById { get; set; }
    public DateTime? SubmittedAtFrom { get; set; }
    public DateTime? SubmittedAtTo { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
}

public sealed record ApprovalRequestSummaryResponse(
    DefaultIdType Id,
    string RequestNumber,
    DefaultIdType WorkflowId,
    string EntityType,
    DefaultIdType EntityId,
    decimal? Amount,
    string Status,
    int CurrentLevel,
    int TotalLevels,
    DateTime SubmittedAt,
    DefaultIdType SubmittedById,
    DateTime? CompletedAt
);
