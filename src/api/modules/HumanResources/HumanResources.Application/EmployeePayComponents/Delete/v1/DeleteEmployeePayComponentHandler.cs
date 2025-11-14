namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Delete.v1;

public sealed class DeleteEmployeePayComponentHandler(
    ILogger<DeleteEmployeePayComponentHandler> logger,
    [FromKeyedServices("hr:employeepaycomponents")] IRepository<EmployeePayComponent> repository)
    : IRequestHandler<DeleteEmployeePayComponentCommand, DeleteEmployeePayComponentResponse>
{
    public async Task<DeleteEmployeePayComponentResponse> Handle(DeleteEmployeePayComponentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assignment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = assignment ?? throw new EmployeePayComponentNotFoundException(request.Id);

        await repository.DeleteAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee pay component with id {AssignmentId} deleted.", assignment.Id);

        return new DeleteEmployeePayComponentResponse(assignment.Id);
    }
}

