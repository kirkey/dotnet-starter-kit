using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Search.v1;

public class SearchLoanApplicationsSpecs : EntitiesByPaginationFilterSpec<LoanApplication, LoanApplicationSummaryResponse>
{
    public SearchLoanApplicationsSpecs(SearchLoanApplicationsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.ApplicationDate, !command.HasOrderBy())
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.LoanProductId == command.LoanProductId!.Value, command.LoanProductId.HasValue)
            .Where(x => x.ApplicationNumber == command.ApplicationNumber, !string.IsNullOrWhiteSpace(command.ApplicationNumber))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.ApplicationDate >= command.ApplicationDateFrom!.Value, command.ApplicationDateFrom.HasValue)
            .Where(x => x.ApplicationDate <= command.ApplicationDateTo!.Value, command.ApplicationDateTo.HasValue)
            .Where(x => x.RequestedAmount >= command.MinRequestedAmount!.Value, command.MinRequestedAmount.HasValue)
            .Where(x => x.RequestedAmount <= command.MaxRequestedAmount!.Value, command.MaxRequestedAmount.HasValue)
            .Where(x => x.AssignedOfficerId == command.AssignedOfficerId!.Value, command.AssignedOfficerId.HasValue);
}
