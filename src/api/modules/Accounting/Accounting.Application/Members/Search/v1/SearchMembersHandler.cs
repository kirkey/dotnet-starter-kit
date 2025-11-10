using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Search.v1;

/// <summary>
/// Handler for searching members with filters and pagination.
/// </summary>
public sealed class SearchMembersHandler(
    [FromKeyedServices("accounting:members")] IReadRepository<Member> repository)
    : IRequestHandler<SearchMembersRequest, PagedList<MemberResponse>>
{
    public async Task<PagedList<MemberResponse>> Handle(SearchMembersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchMembersSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<MemberResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}

