namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;

/// <summary>
/// Request to retrieve a shift assignment.
/// </summary>
public sealed record GetShiftAssignmentRequest(DefaultIdType Id) : IRequest<ShiftAssignmentResponse>;

