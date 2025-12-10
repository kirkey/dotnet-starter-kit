using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Search.v1;

public sealed class SearchBranchesHandler(
    [FromKeyedServices("microfinance:branches")] IReadRepository<Branch> repository)
    : IRequestHandler<SearchBranchesCommand, PagedList<BranchSummaryResponse>>
{
    public async Task<PagedList<BranchSummaryResponse>> Handle(SearchBranchesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBranchesSpec(request);
        var branches = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = branches.Select(b => new BranchSummaryResponse(
            b.Id,
            b.Code,
            b.Name,
            b.BranchType,
            b.Status,
            b.ManagerName,
            b.OpeningDate)).ToList();

        return new PagedList<BranchSummaryResponse>(
            responses,
            request.Filter?.PageNumber ?? 1,
            request.Filter?.PageSize ?? 10,
            totalCount);
    }
}
