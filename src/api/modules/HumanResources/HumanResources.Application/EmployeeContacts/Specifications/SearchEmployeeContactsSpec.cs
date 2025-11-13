using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Specifications;

/// <summary>
/// Specification to search employee contacts with filters.
/// </summary>
public class SearchEmployeeContactsSpec : EntitiesByPaginationFilterSpec<EmployeeContact, EmployeeContactResponse>
{
    public SearchEmployeeContactsSpec(SearchEmployeeContactsRequest request)
        : base(request) =>
        Query
            .Where(c => c.EmployeeId == request.EmployeeId, request.EmployeeId.HasValue)
            .Where(c => c.ContactType == request.ContactType, !string.IsNullOrWhiteSpace(request.ContactType))
            .Where(c => c.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderBy(c => c.Priority, !request.HasOrderBy());
}

