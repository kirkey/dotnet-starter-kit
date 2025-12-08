using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Search.v1;

/// <summary>
/// Handler for searching loan restructures.
/// </summary>
public sealed class SearchLoanRestructuresHandler(
    [FromKeyedServices("microfinance:loanrestructures")] IReadRepository<LoanRestructure> repository)
    : IRequestHandler<SearchLoanRestructuresCommand, PagedList<LoanRestructureSummaryResponse>>
{
    /// <summary>
    /// Handles the search loan restructures command.
    /// </summary>
    public async Task<PagedList<LoanRestructureSummaryResponse>> Handle(
        SearchLoanRestructuresCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanRestructuresSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanRestructureSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
