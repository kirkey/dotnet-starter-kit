using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Search.v1;

/// <summary>
/// Handler for searching employee contacts.
/// </summary>
public sealed class SearchEmployeeContactsHandler(
    [FromKeyedServices("hr:employeecontacts")] IReadRepository<EmployeeContact> repository)
    : IRequestHandler<SearchEmployeeContactsRequest, PagedList<EmployeeContactResponse>>
{
    /// <summary>
    /// Handles the request to search employee contacts.
    /// </summary>
    public async Task<PagedList<EmployeeContactResponse>> Handle(
        SearchEmployeeContactsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchEmployeeContactsSpec(request);
        var contacts = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = contacts.Select(contact => new EmployeeContactResponse
        {
            Id = contact.Id,
            EmployeeId = contact.EmployeeId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            FullName = contact.FullName,
            ContactType = contact.ContactType,
            Relationship = contact.Relationship,
            PhoneNumber = contact.PhoneNumber,
            Email = contact.Email,
            Address = contact.Address,
            Priority = contact.Priority,
            IsActive = contact.IsActive
        }).ToList();

        return new PagedList<EmployeeContactResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

