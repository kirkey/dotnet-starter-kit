namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.End.v1;

/// <summary>
/// Command to end a designation assignment.
/// </summary>
public sealed record EndDesignationAssignmentCommand(
    DefaultIdType Id,
    [property: DefaultValue("2025-01-01")] DateTime EndDate,
    [property: DefaultValue(null)] string? Reason = null)
    : IRequest<EndDesignationAssignmentResponse>;

