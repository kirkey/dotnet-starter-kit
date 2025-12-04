using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Search.v1;

public class SearchMembersCommand : PaginationFilter, IRequest<PagedList<MemberResponse>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MemberNumber { get; set; }
    public DateOnly? DateOfBirthFrom { get; set; }
    public DateOnly? DateOfBirthTo { get; set; }
    public DateOnly? JoinDateFrom { get; set; }
    public DateOnly? JoinDateTo { get; set; }
    public bool? IsActive { get; set; }
}
