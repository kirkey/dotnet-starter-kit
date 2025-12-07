using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Search.v1;

/// <summary>
/// Handler for searching utility members with filters and pagination.
/// </summary>
public sealed class SearchUtilityMembersHandler(
    [FromKeyedServices("accounting:members")] IReadRepository<Member> repository)
    : IRequestHandler<SearchUtilityMembersRequest, PagedList<UtilityMemberResponse>>
{
    public async Task<PagedList<UtilityMemberResponse>> Handle(SearchUtilityMembersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchUtilityMembersSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<UtilityMemberResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}

