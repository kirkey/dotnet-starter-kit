namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;

public sealed class CreateLeaveTypeHandler(
    ILogger<CreateLeaveTypeHandler> logger,
    [FromKeyedServices("hr:leavetypes")] IRepository<LeaveType> repository)
    : IRequestHandler<CreateLeaveTypeCommand, CreateLeaveTypeResponse>
{
    public async Task<CreateLeaveTypeResponse> Handle(
        CreateLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var leaveType = LeaveType.Create(
            request.LeaveName,
            request.AnnualAllowance,
            request.IsPaid,
            request.RequiresApproval);

        leaveType.Update(accrualFrequency: request.AccrualFrequency, description: request.Description);

        if (request.MaxCarryoverDays > 0)
            leaveType.SetCarryoverPolicy(request.MaxCarryoverDays, request.CarryoverExpiryMonths);

        if (request.MinimumNoticeDay.HasValue)
            leaveType.SetMinimumNotice(request.MinimumNoticeDay.Value);

        await repository.AddAsync(leaveType, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Leave type created with ID {LeaveTypeId}, Name {LeaveName}, Annual Allowance {AnnualAllowance} days",
            leaveType.Id,
            request.LeaveName,
            request.AnnualAllowance);

        return new CreateLeaveTypeResponse(leaveType.Id);
    }
}

