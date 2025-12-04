using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Search.v1;

public sealed class SearchLoansHandler(
    [FromKeyedServices("microfinance:loans")] IReadRepository<Loan> repository)
    : IRequestHandler<SearchLoansCommand, PagedList<LoanSummaryResponse>>
{
    public async Task<PagedList<LoanSummaryResponse>> Handle(SearchLoansCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoansSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
