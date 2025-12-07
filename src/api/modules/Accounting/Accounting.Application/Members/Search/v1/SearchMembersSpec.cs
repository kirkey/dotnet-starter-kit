using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Search.v1;

/// <summary>
/// Specification for searching utility members with filtering and pagination.
/// Projects results to <see cref="UtilityMemberResponse"/>.
/// </summary>
public sealed class SearchUtilityMembersSpec : EntitiesByPaginationFilterSpec<Member, UtilityMemberResponse>
{
    public SearchUtilityMembersSpec(SearchUtilityMembersRequest request) : base(request)
    {
        Query
            .OrderBy(m => m.MemberNumber, !request.HasOrderBy())
            .Where(m => m.MemberNumber.Contains(request.MemberNumber!), !string.IsNullOrWhiteSpace(request.MemberNumber))
            .Where(m => m.MemberName.Contains(request.MemberName!), !string.IsNullOrWhiteSpace(request.MemberName))
            .Where(m => m.ServiceAddress.Contains(request.ServiceAddress!), !string.IsNullOrWhiteSpace(request.ServiceAddress))
            .Where(m => m.AccountStatus == request.AccountStatus!, !string.IsNullOrWhiteSpace(request.AccountStatus))
            .Where(m => m.IsActive == request.IsActive!.Value, request.IsActive.HasValue)
            .Where(m => m.ServiceClass == request.ServiceClass!, !string.IsNullOrWhiteSpace(request.ServiceClass));
    }
}

