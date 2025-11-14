namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Delete.v1;

/// <summary>
/// Response for deleting a shift.
/// </summary>
/// <param name="Id">The identifier of the deleted shift.</param>
public sealed record DeleteShiftResponse(DefaultIdType Id);

