using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Search.v1;

public class SearchLoanOfficerAssignmentsSpecs : EntitiesByPaginationFilterSpec<LoanOfficerAssignment, LoanOfficerAssignmentSummaryResponse>
{
    public SearchLoanOfficerAssignmentsSpecs(SearchLoanOfficerAssignmentsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.AssignmentDate, !command.HasOrderBy())
            .Where(x => x.StaffId == command.StaffId!.Value, command.StaffId.HasValue)
            .Where(x => x.AssignmentType == command.AssignmentType, !string.IsNullOrWhiteSpace(command.AssignmentType))
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.MemberGroupId == command.MemberGroupId!.Value, command.MemberGroupId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.BranchId == command.BranchId!.Value, command.BranchId.HasValue)
            .Where(x => x.AssignmentDate >= command.AssignmentDateFrom!.Value, command.AssignmentDateFrom.HasValue)
            .Where(x => x.AssignmentDate <= command.AssignmentDateTo!.Value, command.AssignmentDateTo.HasValue)
            .Where(x => x.IsPrimary == command.IsPrimary!.Value, command.IsPrimary.HasValue)
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status));
}
