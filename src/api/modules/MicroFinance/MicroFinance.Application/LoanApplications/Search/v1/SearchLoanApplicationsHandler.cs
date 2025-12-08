using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Search.v1;

public sealed class SearchLoanApplicationsHandler(
    [FromKeyedServices("microfinance:loanapplications")] IReadRepository<LoanApplication> repository)
    : IRequestHandler<SearchLoanApplicationsCommand, PagedList<LoanApplicationSummaryResponse>>
{
    public async Task<PagedList<LoanApplicationSummaryResponse>> Handle(
        SearchLoanApplicationsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanApplicationsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanApplicationSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
