using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Search.v1;

public sealed class SearchLoanOfficerAssignmentsHandler(
    [FromKeyedServices("microfinance:loanofficerassignments")] IReadRepository<LoanOfficerAssignment> repository)
    : IRequestHandler<SearchLoanOfficerAssignmentsCommand, PagedList<LoanOfficerAssignmentSummaryResponse>>
{
    public async Task<PagedList<LoanOfficerAssignmentSummaryResponse>> Handle(
        SearchLoanOfficerAssignmentsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanOfficerAssignmentsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanOfficerAssignmentSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
