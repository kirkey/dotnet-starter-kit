using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Search.v1;

public class SearchLoanOfficerAssignmentsCommand : PaginationFilter, IRequest<PagedList<LoanOfficerAssignmentSummaryResponse>>
{
    public DefaultIdType? StaffId { get; set; }
    public string? AssignmentType { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? MemberGroupId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public DateOnly? AssignmentDateFrom { get; set; }
    public DateOnly? AssignmentDateTo { get; set; }
    public bool? IsPrimary { get; set; }
    public string? Status { get; set; }
}

public sealed record LoanOfficerAssignmentSummaryResponse(
    DefaultIdType Id,
    DefaultIdType StaffId,
    string AssignmentType,
    DefaultIdType? MemberId,
    DefaultIdType? MemberGroupId,
    DefaultIdType? LoanId,
    DefaultIdType? BranchId,
    DateOnly AssignmentDate,
    DateOnly? EndDate,
    DefaultIdType? PreviousStaffId,
    string? Reason,
    bool IsPrimary,
    string Status
);
