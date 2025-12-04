using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Search.v1;

/// <summary>
/// Handler for searching loan repayments with filters and pagination.
/// </summary>
public sealed class SearchLoanRepaymentsHandler(
    ILogger<SearchLoanRepaymentsHandler> logger,
    [FromKeyedServices("microfinance:loanrepayments")] IReadRepository<LoanRepayment> repository)
    : IRequestHandler<SearchLoanRepaymentsCommand, PagedList<LoanRepaymentResponse>>
{
    public async Task<PagedList<LoanRepaymentResponse>> Handle(SearchLoanRepaymentsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanRepaymentsSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Searched loan repayments with {Count} results", items.Count);

        return new PagedList<LoanRepaymentResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
