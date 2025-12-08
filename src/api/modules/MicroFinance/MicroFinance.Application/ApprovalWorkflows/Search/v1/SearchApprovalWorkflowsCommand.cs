using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalWorkflows.Search.v1;

public class SearchApprovalWorkflowsCommand : PaginationFilter, IRequest<PagedList<ApprovalWorkflowSummaryResponse>>
{
    public string? Code { get; set; }
    public string? EntityType { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsSequential { get; set; }
}

public sealed record ApprovalWorkflowSummaryResponse(
    DefaultIdType Id,
    string Code,
    string EntityType,
    decimal? MinAmount,
    decimal? MaxAmount,
    int NumberOfLevels,
    bool IsSequential,
    bool IsActive,
    int Priority);
