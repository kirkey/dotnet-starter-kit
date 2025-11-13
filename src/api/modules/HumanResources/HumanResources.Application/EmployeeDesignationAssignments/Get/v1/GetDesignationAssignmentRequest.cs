namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Get.v1;

/// <summary>
/// Request to get designation assignment by ID.
/// </summary>
public sealed record GetDesignationAssignmentRequest(DefaultIdType Id) 
    : IRequest<DesignationAssignmentResponse>;

