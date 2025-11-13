namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Delete.v1;

/// <summary>
/// Handler for deleting employee dependent.
/// </summary>
public sealed class DeleteEmployeeDependentHandler(
    ILogger<DeleteEmployeeDependentHandler> logger,
    [FromKeyedServices("hr:dependents")] IRepository<EmployeeDependent> repository)
    : IRequestHandler<DeleteEmployeeDependentCommand, DeleteEmployeeDependentResponse>
{
    public async Task<DeleteEmployeeDependentResponse> Handle(
        DeleteEmployeeDependentCommand request,
        CancellationToken cancellationToken)
    {
        var dependent = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (dependent is null)
            throw new EmployeeDependentNotFoundException(request.Id);

        await repository.DeleteAsync(dependent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee dependent {DependentId} deleted successfully", dependent.Id);

        return new DeleteEmployeeDependentResponse(dependent.Id);
    }
}

