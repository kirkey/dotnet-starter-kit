namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

/// <summary>
/// Request to get a shift by its identifier.
/// </summary>
public sealed record GetShiftRequest(DefaultIdType Id) : IRequest<ShiftResponse>;


