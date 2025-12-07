using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Search.v1;

/// <summary>
/// Request to search for utility members with optional filters and pagination.
/// </summary>
public sealed class SearchUtilityMembersRequest : PaginationFilter, IRequest<PagedList<UtilityMemberResponse>>
{
    public string? MemberNumber { get; init; }
    public string? MemberName { get; init; }
    public string? ServiceAddress { get; init; }
    public string? AccountStatus { get; init; }
    public bool? IsActive { get; init; }
    public string? ServiceClass { get; init; }
}

