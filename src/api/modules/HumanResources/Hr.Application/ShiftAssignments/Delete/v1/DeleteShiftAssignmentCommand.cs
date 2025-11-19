namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Delete.v1;

/// <summary>
/// Command to delete a shift assignment.
/// </summary>
public sealed record DeleteShiftAssignmentCommand(DefaultIdType Id) : IRequest<DeleteShiftAssignmentResponse>;

