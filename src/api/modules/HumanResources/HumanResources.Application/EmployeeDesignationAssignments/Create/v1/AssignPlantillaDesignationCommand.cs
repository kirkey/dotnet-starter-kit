namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Create.v1;

/// <summary>
/// Command to assign a plantilla (primary) designation to an employee.
/// </summary>
public sealed record AssignPlantillaDesignationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType DesignationId,
    [property: DefaultValue("2025-01-01")] DateTime EffectiveDate,
    [property: DefaultValue(null)] string? Reason = null,
    [property: DefaultValue(null)] DefaultIdType? PreviousAssignmentId = null) : IRequest<AssignDesignationResponse>;

