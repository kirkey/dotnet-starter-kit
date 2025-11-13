using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

/// <summary>
/// Handler for getting employee contact by ID.
/// </summary>
public sealed class GetEmployeeContactHandler(
    [FromKeyedServices("hr:contacts")] IReadRepository<EmployeeContact> repository)
    : IRequestHandler<GetEmployeeContactRequest, EmployeeContactResponse>
{
    public async Task<EmployeeContactResponse> Handle(
        GetEmployeeContactRequest request,
        CancellationToken cancellationToken)
    {
        var contact = await repository
            .FirstOrDefaultAsync(new EmployeeContactByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (contact is null)
            throw new EmployeeContactNotFoundException(request.Id);

        return new EmployeeContactResponse
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
        };
    }
}

