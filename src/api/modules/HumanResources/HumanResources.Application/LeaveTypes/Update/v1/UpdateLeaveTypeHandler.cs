namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Update.v1;

public sealed class UpdateLeaveTypeHandler(
    ILogger<UpdateLeaveTypeHandler> logger,
    [FromKeyedServices("hr:leavetypes")] IRepository<LeaveType> repository)
    : IRequestHandler<UpdateLeaveTypeCommand, UpdateLeaveTypeResponse>
{
    public async Task<UpdateLeaveTypeResponse> Handle(
        UpdateLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        var leaveType = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (leaveType is null)
            throw new LeaveTypeNotFoundException(request.Id);

        if (request.AnnualAllowance.HasValue)
            leaveType.Update(annualAllowance: request.AnnualAllowance.Value);

        if (request.MaxCarryoverDays.HasValue)
            leaveType.SetCarryoverPolicy(request.MaxCarryoverDays.Value);

        if (!string.IsNullOrWhiteSpace(request.Description))
            leaveType.Update(description: request.Description);

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                leaveType.Activate();
            else
                leaveType.Deactivate();
        }

        await repository.UpdateAsync(leaveType, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Leave type {LeaveTypeId} updated successfully", leaveType.Id);

        return new UpdateLeaveTypeResponse(leaveType.Id);
    }
}

