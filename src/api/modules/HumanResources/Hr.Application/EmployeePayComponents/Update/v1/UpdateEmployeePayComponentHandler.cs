namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Update.v1;

public sealed class UpdateEmployeePayComponentHandler(
    ILogger<UpdateEmployeePayComponentHandler> logger,
    [FromKeyedServices("hr:employeepaycomponents")] IRepository<EmployeePayComponent> repository)
    : IRequestHandler<UpdateEmployeePayComponentCommand, UpdateEmployeePayComponentResponse>
{
    public async Task<UpdateEmployeePayComponentResponse> Handle(UpdateEmployeePayComponentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assignment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = assignment ?? throw new EmployeePayComponentNotFoundException(request.Id);

        // Update using the entity's Update method
        assignment.Update(
            customRate: request.CustomRate,
            fixedAmount: request.FixedAmount,
            customFormula: request.CustomFormula,
            remarks: request.Remarks);

        await repository.UpdateAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee pay component with id {AssignmentId} updated.", assignment.Id);

        return new UpdateEmployeePayComponentResponse(assignment.Id);
    }
}

