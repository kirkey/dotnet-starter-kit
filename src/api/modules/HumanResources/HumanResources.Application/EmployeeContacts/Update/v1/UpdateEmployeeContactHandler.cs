namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Update.v1;

/// <summary>
/// Handler for updating employee contact.
/// </summary>
public sealed class UpdateEmployeeContactHandler(
    ILogger<UpdateEmployeeContactHandler> logger,
    [FromKeyedServices("hr:contacts")] IRepository<EmployeeContact> repository)
    : IRequestHandler<UpdateEmployeeContactCommand, UpdateEmployeeContactResponse>
{
    public async Task<UpdateEmployeeContactResponse> Handle(
        UpdateEmployeeContactCommand request,
        CancellationToken cancellationToken)
    {
        var contact = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (contact is null)
            throw new EmployeeContactNotFoundException(request.Id);

        contact.Update(
            request.FirstName,
            request.LastName,
            request.Relationship,
            request.PhoneNumber,
            request.Email,
            request.Address);

        if (request.Priority.HasValue)
            contact.SetPriority(request.Priority.Value);

        await repository.UpdateAsync(contact, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee contact {ContactId} updated successfully", contact.Id);

        return new UpdateEmployeeContactResponse(contact.Id);
    }
}

