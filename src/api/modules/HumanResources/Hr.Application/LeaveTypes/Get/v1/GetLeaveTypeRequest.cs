namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;

/// <summary>
/// Request to get a leave type by its identifier.
/// </summary>
public sealed record GetLeaveTypeRequest(DefaultIdType Id) : IRequest<LeaveTypeResponse>;

