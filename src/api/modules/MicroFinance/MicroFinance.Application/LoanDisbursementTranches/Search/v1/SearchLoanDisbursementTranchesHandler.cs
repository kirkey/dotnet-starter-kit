using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Search.v1;

public sealed class SearchLoanDisbursementTranchesHandler(
    [FromKeyedServices("microfinance:loandisbursementtranches")] IReadRepository<LoanDisbursementTranche> repository)
    : IRequestHandler<SearchLoanDisbursementTranchesCommand, PagedList<LoanDisbursementTrancheSummaryResponse>>
{
    public async Task<PagedList<LoanDisbursementTrancheSummaryResponse>> Handle(
        SearchLoanDisbursementTranchesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanDisbursementTranchesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanDisbursementTrancheSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
