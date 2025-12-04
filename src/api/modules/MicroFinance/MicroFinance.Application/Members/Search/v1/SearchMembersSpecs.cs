using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Search.v1;

public class SearchMembersSpecs : EntitiesByPaginationFilterSpec<Member, MemberResponse>
{
    public SearchMembersSpecs(SearchMembersCommand command)
        : base(command) =>
        Query
            .OrderBy(m => m.MemberNumber, !command.HasOrderBy())
            .Where(m => m.FirstName.Contains(command.FirstName!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.FirstName))
            .Where(m => m.LastName.Contains(command.LastName!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.LastName))
            .Where(m => m.MemberNumber == command.MemberNumber, !string.IsNullOrWhiteSpace(command.MemberNumber))
            .Where(m => m.DateOfBirth >= command.DateOfBirthFrom!.Value, command.DateOfBirthFrom.HasValue)
            .Where(m => m.DateOfBirth <= command.DateOfBirthTo!.Value, command.DateOfBirthTo.HasValue)
            .Where(m => m.JoinDate >= command.JoinDateFrom!.Value, command.JoinDateFrom.HasValue)
            .Where(m => m.JoinDate <= command.JoinDateTo!.Value, command.JoinDateTo.HasValue)
            .Where(m => m.IsActive == command.IsActive!.Value, command.IsActive.HasValue);
}
