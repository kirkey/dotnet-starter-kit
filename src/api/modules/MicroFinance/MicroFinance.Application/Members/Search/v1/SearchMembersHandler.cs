using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Search.v1;

public sealed class SearchMembersHandler(
    [FromKeyedServices("microfinance:members")] IReadRepository<Member> repository)
    : IRequestHandler<SearchMembersCommand, PagedList<MemberResponse>>
{
    public async Task<PagedList<MemberResponse>> Handle(SearchMembersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMembersSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<MemberResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
