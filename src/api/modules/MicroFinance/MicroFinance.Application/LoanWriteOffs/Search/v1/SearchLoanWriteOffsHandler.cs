using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Search.v1;

/// <summary>
/// Handler for searching loan write-offs.
/// </summary>
public sealed class SearchLoanWriteOffsHandler(
    [FromKeyedServices("microfinance:loanwriteoffs")] IReadRepository<LoanWriteOff> repository)
    : IRequestHandler<SearchLoanWriteOffsCommand, PagedList<LoanWriteOffSummaryResponse>>
{
    /// <summary>
    /// Handles the search loan write-offs command.
    /// </summary>
    public async Task<PagedList<LoanWriteOffSummaryResponse>> Handle(
        SearchLoanWriteOffsCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanWriteOffsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanWriteOffSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
