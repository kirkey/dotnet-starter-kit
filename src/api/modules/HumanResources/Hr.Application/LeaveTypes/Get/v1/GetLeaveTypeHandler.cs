using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;

/// <summary>
/// Handler for retrieving a leave type by ID.
/// </summary>
public sealed class GetLeaveTypeHandler(
    [FromKeyedServices("hr:leavetypes")] IReadRepository<LeaveType> repository)
    : IRequestHandler<GetLeaveTypeRequest, LeaveTypeResponse>
{
    public async Task<LeaveTypeResponse> Handle(
        GetLeaveTypeRequest request,
        CancellationToken cancellationToken)
    {
        var leaveType = await repository
            .FirstOrDefaultAsync(new LeaveTypeByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (leaveType is null)
            throw new LeaveTypeNotFoundException(request.Id);

        return new LeaveTypeResponse
        {
            Id = leaveType.Id,
            LeaveName = leaveType.LeaveName,
            AnnualAllowance = leaveType.AnnualAllowance,
            AccrualFrequency = leaveType.AccrualFrequency,
            IsPaid = leaveType.IsPaid,
            MaxCarryoverDays = leaveType.MaxCarryoverDays,
            CarryoverExpiryMonths = leaveType.CarryoverExpiryMonths,
            RequiresApproval = leaveType.RequiresApproval,
            MinimumNoticeDay = leaveType.MinimumNoticeDay,
            IsActive = leaveType.IsActive,
            Description = leaveType.Description
        };
    }
}

