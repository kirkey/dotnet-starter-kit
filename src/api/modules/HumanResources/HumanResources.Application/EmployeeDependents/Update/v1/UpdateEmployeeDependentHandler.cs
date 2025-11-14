namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;

/// <summary>
/// Handler for updating an employee dependent.
/// </summary>
public sealed class UpdateEmployeeDependentHandler(
    ILogger<UpdateEmployeeDependentHandler> logger,
    [FromKeyedServices("hr:dependents")] IRepository<EmployeeDependent> repository)
    : IRequestHandler<UpdateEmployeeDependentCommand, UpdateEmployeeDependentResponse>
{
    /// <summary>
    /// Handles the request to update an employee dependent.
    /// </summary>
    public async Task<UpdateEmployeeDependentResponse> Handle(
        UpdateEmployeeDependentCommand request,
        CancellationToken cancellationToken)
    {
        var dependent = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (dependent is null)
            throw new EmployeeDependentNotFoundException(request.Id);

        dependent.Update(
            request.FirstName,
            request.LastName,
            request.Relationship,
            request.Email,
            request.PhoneNumber);

        if (request.IsBeneficiary.HasValue)
            dependent.SetAsBeneficiary(request.IsBeneficiary.Value);

        if (request.IsClaimableDependent.HasValue)
            dependent.SetAsClaimableDependent(request.IsClaimableDependent.Value);

        await repository.UpdateAsync(dependent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee dependent {DependentId} updated successfully", dependent.Id);

        return new UpdateEmployeeDependentResponse(dependent.Id);
    }
}

