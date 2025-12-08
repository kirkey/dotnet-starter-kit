using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Search.v1;

public sealed class SearchLoanOfficerTargetsHandler(
    [FromKeyedServices("microfinance:loanofficertargets")] IReadRepository<LoanOfficerTarget> repository)
    : IRequestHandler<SearchLoanOfficerTargetsCommand, PagedList<LoanOfficerTargetSummaryResponse>>
{
    public async Task<PagedList<LoanOfficerTargetSummaryResponse>> Handle(
        SearchLoanOfficerTargetsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLoanOfficerTargetsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<LoanOfficerTargetSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
