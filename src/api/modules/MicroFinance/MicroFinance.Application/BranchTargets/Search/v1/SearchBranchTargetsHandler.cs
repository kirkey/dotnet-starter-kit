using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Search.v1;

public sealed class SearchBranchTargetsHandler(
    [FromKeyedServices("microfinance:branchtargets")] IReadRepository<BranchTarget> repository)
    : IRequestHandler<SearchBranchTargetsCommand, PagedList<BranchTargetSummaryResponse>>
{
    public async Task<PagedList<BranchTargetSummaryResponse>> Handle(
        SearchBranchTargetsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBranchTargetsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<BranchTargetSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
