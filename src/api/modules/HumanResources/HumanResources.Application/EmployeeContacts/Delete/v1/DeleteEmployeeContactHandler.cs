namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Delete.v1;

/// <summary>
/// Handler for deleting employee contact.
/// </summary>
public sealed class DeleteEmployeeContactHandler(
    ILogger<DeleteEmployeeContactHandler> logger,
    [FromKeyedServices("hr:contacts")] IRepository<EmployeeContact> repository)
    : IRequestHandler<DeleteEmployeeContactCommand, DeleteEmployeeContactResponse>
{
    public async Task<DeleteEmployeeContactResponse> Handle(
        DeleteEmployeeContactCommand request,
        CancellationToken cancellationToken)
    {
        var contact = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (contact is null)
            throw new EmployeeContactNotFoundException(request.Id);

        await repository.DeleteAsync(contact, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee contact {ContactId} deleted successfully", contact.Id);

        return new DeleteEmployeeContactResponse(contact.Id);
    }
}

